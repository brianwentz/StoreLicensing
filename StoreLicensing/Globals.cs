// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Resources;

namespace StoreLicensing.Powershell
{
    internal static class Globals
    {
        private static ResourceManager _resources = null;
        public static ResourceManager Resources
        {
            get
            {
                if (_resources == null)
                {
                    _resources = new ResourceManager("StoreLicensing.Powershell.Properties.Resources", typeof(Globals).Assembly);
                }
                return _resources;
            }
        }
    }
}