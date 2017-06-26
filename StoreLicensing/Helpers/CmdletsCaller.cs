// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace StoreLicensing.Powershell
{
    internal class CmdletsCaller
    {
        private Runspace _runSpace;
        private static CmdletsCaller _instance = new CmdletsCaller();

        private CmdletsCaller()
        {
            _runSpace = RunspaceFactory.CreateRunspace();
            _runSpace.Open();
        }

        /// <summary>Calling command1 | command2 | ... | commandn</summary>
        /// <param name="commands">Each command given as a string</param>
        /// <returns>The result as <see cref="Collection{PSObject}"/></returns>
        public Collection<PSObject> ExecuteCmdlet(params string[] commands)
        {
            Pipeline pipeline = _runSpace.CreatePipeline();

            foreach (string command in commands)
            {
                string[] commandParts = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (commandParts.Length > 0)
                {
                    Command cmd = new Command(commandParts[0]);

                    for (int i = 1; i < commandParts.Length; ++i)
                    {
                        // Parameters can be of type <Name, Value> (-Path C:\myPath) or just <Name> (-Force)
                        if (commandParts[i].StartsWith("-"))
                        {
                            // Look at the next word to determine if this is <Name, Value> or just Name
                            if ((i+1 < commandParts.Length) && (!commandParts[i+1].StartsWith("-")))
                            {
                                cmd.Parameters.Add(commandParts[i].Substring(1), commandParts[i + 1]);
                                ++i; // Do not process next word because we used it here
                            }
                            else
                            {
                                cmd.Parameters.Add(commandParts[i].Substring(1));
                            }
                        }
                        else
                        {
                            throw new Exception("Parameter name must be explicitly used in command. Ex. New-Item -Path SomePath");
                        }
                    }

                    pipeline.Commands.Add(cmd);
                }
            }

            return pipeline.Invoke();
        }

        public static CmdletsCaller GetInstance()
        {
            return _instance;
        }
    }
}
