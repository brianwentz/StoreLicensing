// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace StoreLicensing.UnitTests
{
    internal class CommandRuntimeWriteEvent
    {
        public enum WriteKind
        {
            Detail,
            Debug,
            Error,
            Object, 
            Progress,
            Verbose,
            Warning
        };
        public CommandRuntimeWriteEvent(WriteKind kind, object data)
        {
            Kind = kind;
            Data = data;
        }
        public WriteKind Kind { get; private set; }
        public object Data { get; private set; }

        public static void AddWriteEvent(List<CommandRuntimeWriteEvent> eventList, CommandRuntimeWriteEvent.WriteKind kind, object data)
        {
            CommandRuntimeWriteEvent writeEvent = new CommandRuntimeWriteEvent(kind, data);
            eventList.Add(writeEvent);            
        }
    }
}
