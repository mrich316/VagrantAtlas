using System;
using System.Collections.Generic;

namespace VagrantAtlas.WebApi
{
    public static class Atlas
    {
        private static readonly List<Box> _atlas = new List<Box>
        {
            new Box
            {
                User = "ggp",
                Name = "windows_2012_r2",
                Description = "Windows 2012 R2 with patches",
                Tags = new HashSet<string> { "windows" },
                Versions = new List<BoxVersion>
                {
                    new BoxVersion
                    {
                        Version = "20160520.0.0",
                        Providers = new List<BoxProvider>
                        {
                            new BoxProvider
                            {
                                Name = "virtualbox",
                                Url = "http://vwsdags02.info.ville.laval.qc.ca/files/windows_2012_r2_virtualbox.box",
                                ChecksumType = "sha1",
                                Checksum = "30ccd5a341d616518fb153cce16580b98969aafd"
                            }
                        }
                    }
                }
            }
        };

        public static List<Box> Boxes
        {
            get { return _atlas; }
        }
    }
}