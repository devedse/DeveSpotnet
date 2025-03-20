using System.Xml;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class NewznabApiHelpers
    {
        public static string Cat2NewznabCat(int hcat, string cat, string catZ)
        {
            string[] catList = cat.Split('|');
            cat = catList[0];
            string nr = cat.Substring(1);
            string[] zcatList = catZ.Split('|');
            string zcat = zcatList[0];
            string znr = zcat.Substring(1);
            //znr = Math.Min(4, int.Parse(znr));
            if (znr == "2")
            {
                znr = "0";
            }

            // Als $nr niet gevonden kan worden is dat niet erg, het mag echter
            // geen Exception veroorzaken.
            if (!string.IsNullOrEmpty(cat) && cat.Length > 0)
            {
                switch (cat[0])
                {
                    case 'a':
                        if (hcat == 0 || hcat == 1)
                        {
                            var newznabcat = SpotAcat2nabcat();
                            string r = null;
                            try
                            {
                                r = newznabcat[hcat][int.Parse(znr)][nr];
                            }
                            catch
                            {
                                // ignore
                            }
                            if (r == null)
                            {
                                r = "";
                            }

                            return r;
                        }
                        else
                        {
                            var newznabcat = SpotAcat2nabcat();
                            string r = null;
                            try
                            {
                                r = newznabcat[hcat][int.Parse(znr)][nr];
                            }
                            catch
                            {
                                // ignore
                            }
                            if (r == null)
                            {
                                r = "";
                            }

                            return r;
                        }
                    case 'b':
                        var newznabcatB = SpotBcat2nabcat();
                        try
                        {
                            return newznabcatB.ContainsKey(nr) ? newznabcatB[nr] : "";
                        }
                        catch
                        {
                            return "";
                        }
                }
            }

            return "";
        }

        public static void ShowApiError(int errcode = 42)
        {
            string errtext;
            switch (errcode)
            {
                case 100: errtext = "Incorrect user credentials"; break;
                case 101: errtext = "Account suspended"; break;
                case 102: errtext = "Insufficient priviledges/not authorized"; break;
                case 103: errtext = "Registration denied"; break;
                case 104: errtext = "Registrations are closed"; break;
                case 105: errtext = "Invalid registration (Email Address Taken)"; break;
                case 106: errtext = "Invalid registration (Email Address Bad Format)"; break;
                case 107: errtext = "Registration Failed (Data error)"; break;

                case 200: errtext = "Missing parameter"; break;
                case 201: errtext = "Incorrect parameter"; break;
                case 202: errtext = "No such function"; break;
                case 203: errtext = "Function not available"; break;

                case 300: errtext = "On TVSearch no q, tvmaze or rid parameter present"; break;
                case 301: errtext = "IMDB information returned is invalid"; break;
                case 302: errtext = "Error in fetching spot information"; break;

                case 500: errtext = "Request limit reached"; break;
                case 501: errtext = "Download limit reached"; break;
                default: errtext = "Unknown error"; break;
            }

            XmlDocument doc = new XmlDocument();
            doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.PreserveWhitespace = true;

            XmlElement error = doc.CreateElement("error");
            error.SetAttribute("code", errcode.ToString());
            error.SetAttribute("description", errtext);
            doc.AppendChild(error);

            // In a real implementation, you would send the content type header here
            // Context.Response.ContentType = "text/xml";
            // Context.Response.Write(doc.OuterXml);
        }

        public static List<Dictionary<string, object>> Categories()
        {
            var categories = new List<Dictionary<string, object>>();

            // Console
            var console = new Dictionary<string, object>
            {
                { "name", "Console" },
                { "cat", "1000" },
                { "subcata", new Dictionary<string, string>
                    {
                        { "NDS", "1010" },
                        { "PSP", "1020" },
                        { "Wii", "1030" },
                        { "Xbox", "1040" },
                        { "Xbox 360", "1050" },
                        { "PS3", "1080" }
                    }
                }
            };
            categories.Add(console);

            // Movies
            var movies = new Dictionary<string, object>
            {
                { "name", "Movies" },
                { "cat", "2000" },
                { "subcata", new Dictionary<string, string>
                    {
                        { "SD", "2030" },
                        { "HD", "2040" },
                        { "BluRay", "2050" },
                        { "3D", "2060" }
                    }
                }
            };
            categories.Add(movies);

            // Audio
            var audio = new Dictionary<string, object>
            {
                { "name", "Audio" },
                { "cat", "3000" },
                { "subcata", new Dictionary<string, string>
                    {
                        { "MP3", "3010" },
                        { "Video", "3020" },
                        { "Lossless", "3040" }
                    }
                }
            };
            categories.Add(audio);

            // PC
            var pc = new Dictionary<string, object>
            {
                { "name", "PC" },
                { "cat", "4000" },
                { "subcata", new Dictionary<string, string>
                    {
                        { "Windows", "4020" },
                        { "Mac", "4030" },
                        { "Mobile", "4040" },
                        { "Games", "4050" }
                    }
                }
            };
            categories.Add(pc);

            // TV
            var tv = new Dictionary<string, object>
            {
                { "name", "TV" },
                { "cat", "5000" },
                { "subcata", new Dictionary<string, string>
                    {
                        { "Foreign", "5020" },
                        { "SD", "5030" },
                        { "HD", "5040" },
                        { "Other", "5050" },
                        { "Sport", "5060" },
                        { "Anime", "5070" }
                    }
                }
            };
            categories.Add(tv);

            // XXX
            var xxx = new Dictionary<string, object>
            {
                { "name", "XXX" },
                { "cat", "6000" },
                { "subcata", new Dictionary<string, string>
                    {
                        { "DVD", "6010" },
                        { "WMV", "6020" },
                        { "XviD", "6030" },
                        { "x264", "6040" }
                    }
                }
            };
            categories.Add(xxx);

            // Other
            var other = new Dictionary<string, object>
            {
                { "name", "Other" },
                { "cat", "7000" },
                { "subcata", new Dictionary<string, string>
                    {
                        { "Misc", "7010" },
                        { "Ebook", "7020" }
                    }
                }
            };
            categories.Add(other);

            return categories;
        }

        public static string Nabcat2spotcat(int cat)
        {
            switch (cat)
            {
                case 1000: return "cat2_a3,cat2_a4,cat2_a5,cat2_a6,cat2_a7,cat2_a8,cat2_a9,cat2_a10,cat2_a11,cat2_a12";
                case 1010: return "cat2_a10";
                case 1020: return "cat2_a5";
                case 1030: return "cat2_a11";
                case 1040: return "cat2_a6";
                case 1050: return "cat2_a7";
                case 1060: return "cat2_a7";

                case 2000: return "cat0_z0";
                case 2010:
                case 2030: return "cat0_z0_a0,cat0_z0_a1,cat0_z0_a2,cat0_z0_a3,cat0_z0_a10";  // Movies/SD
                case 2040: return "cat0_z0_a4,cat0_z0_a7,cat0_z0_a8,cat0_z0_a9";              // Movies/HD
                case 2050: return "cat0_z0_a6";                                               // Movies/BluRay
                case 2060: return "cat0_z0_a14";                                              // Movies/3D

                case 3000: return "cat1_a";
                case 3010: return "cat1_a0";
                case 3020: return "cat0_d13";
                case 3030: return "cat1_z3";
                case 3040: return "cat1_a2,cat1_a4,cat1_a7,cat1_a8";

                case 4000: return "cat3";
                case 4020: return "cat3_a0";
                case 4030: return "cat3_a1";
                case 4040: return "cat3_a4,cat3_a5,cat3_a6,cat3_a7";
                case 4050: return "cat2_a";

                case 5000: return "cat0_z1,cat0_z1_d";
                case 5020: return "cat0_z1,cat0_z1_d";
                case 5030: return "cat0_z1_a0,cat0_z1_a1,cat0_z1_a2,cat0_z1_a3,cat0_z1_a10,cat0_z1_d";
                case 5040: return "cat0_z1_a4,cat0_z1_a6,cat0_z1_a7,cat0_z1_a8,cat0_z1_a9,cat0_z1_a14,cat0_z1_a15,cat0_z1_d";
                case 5050: return "cat0_z1,cat0_z1_d";
                case 5060: return "cat0_z1_d18";
                case 5070: return "cat0_z1_d29";

                case 6000: return "cat0_z3";
                case 6010: return "cat0_z3_a3,cat0_z3_a10";
                case 6020: return "cat0_z3_a1,cat0_z3_a8";
                case 6030: return "cat0_z3_a0";
                case 6040: return "cat0_z3_a4,cat0_z3_a6,cat0_z3_a7,cat0_z3_a8,cat0_z3_a9";

                case 7020: return "cat0_z2";
            }

            return "";
        }

        public static Dictionary<int, Dictionary<int, Dictionary<string, string>>> SpotAcat2nabcat()
        {
            var result = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();

            // Cat0 - Image
            var cat0 = new Dictionary<int, Dictionary<string, string>>();

            // Z0 - Movie
            var cat0z0 = new Dictionary<string, string>
            {
                { "0", "2000|2030" },
                { "1", "2000|2030" },
                { "2", "2000|2030" },
                { "3", "2000|2030" },
                { "4", "2000|2040" },
                { "5", "7000|7020" },
                { "6", "2000|2050" },
                { "7", "2000|2040" },
                { "8", "2000|2040" },
                { "9", "2000|2040" },
                { "10", "2000|2030" },
                { "11", "7000|7020" },
                { "14", "2000|2060" }
            };
            cat0.Add(0, cat0z0);

            // Z1 - Series
            var cat0z1 = new Dictionary<string, string>
            {
                { "0", "5000|5030" },
                { "1", "5000|5030" },
                { "2", "5000|5030" },
                { "3", "5000|5030" },
                { "4", "5000|5040" },
                { "5", "7000|7020" },
                { "6", "5000|5040" },
                { "7", "5000|5040" },
                { "8", "5000|5040" },
                { "9", "5000|5040" },
                { "10", "5000|5030" },
                { "11", "7000|7020" },
                { "12", "8000|8000" },
                { "13", "8000|8000" },
                { "14", "5000|5050" },
                { "15", "5000|5045" }
            };
            cat0.Add(1, cat0z1);

            // Z3 - Erotic
            var cat0z3 = new Dictionary<string, string>
            {
                { "0", "6000|6030" },
                { "1", "6000|6030" },
                { "2", "6000|6030" },
                { "3", "6000|6030" },
                { "4", "6000|6040" },
                { "5", "6000|6000" },
                { "6", "6000|6040" },
                { "7", "6000|6040" },
                { "8", "6000|6030" },
                { "9", "6000|6040" },
                { "10", "6000|6030" },
                { "11", "6000|6000" }
            };
            cat0.Add(3, cat0z3);

            // Z4 Picture (new)
            var cat0z4 = new Dictionary<string, string>
            {
                { "11", "7000|7010" },
                { "12", "7000|7010" }
            };
            cat0.Add(4, cat0z4);

            result.Add(0, cat0);

            // Cat1 - Audio
            var cat1 = new Dictionary<int, Dictionary<string, string>>();

            // Z0
            var cat1z0 = new Dictionary<string, string>
            {
                { "0", "3000|3010" },  // MP3
                { "1", "3000|3010" },  // WMA
                { "2", "3000|3040" },  // WAV
                { "3", "3000|3010" },  // OGG
                { "4", "3000|3040" },  // EAC
                { "5", "3000|3040" },  // DTS
                { "6", "3000|3010" },  // AAC
                { "7", "3000|3040" },  // APE
                { "8", "3000|3040" }   // FLAC
            };
            cat1.Add(0, cat1z0);

            // Z1
            var cat1z1 = new Dictionary<string, string>
            {
                { "0", "3000|3010" },  // MP3
                { "1", "3000|3010" },  // WMA
                { "2", "3000|3040" },  // WAV
                { "3", "3000|3010" },  // OGG
                { "4", "3000|3040" },  // EAC
                { "5", "3000|3040" },  // DTS
                { "6", "3000|3010" },  // AAC
                { "7", "3000|3040" },  // APE
                { "8", "3000|3040" }   // FLAC
            };
            cat1.Add(1, cat1z1);

            // Z2
            var cat1z2 = new Dictionary<string, string>
            {
                { "0", "3000|3010" },  // MP3
                { "1", "3000|3010" },  // WMA
                { "2", "3000|3040" },  // WAV
                { "3", "3000|3010" },  // OGG
                { "4", "3000|3040" },  // EAC
                { "5", "3000|3040" },  // DTS
                { "6", "3000|3010" },  // AAC
                { "7", "3000|3040" },  // APE
                { "8", "3000|3040" }   // FLAC
            };
            cat1.Add(2, cat1z2);

            // Z3
            var cat1z3 = new Dictionary<string, string>
            {
                { "0", "3000|3030" },  // MP3
                { "1", "3000|3030" },  // WMA
                { "2", "3000|3030" },  // WAV
                { "3", "3000|3030" },  // OGG
                { "4", "3000|3030" },  // EAC
                { "5", "3000|3030" },  // DTS
                { "6", "3000|3030" },  // AAC
                { "7", "3000|3030" },  // APE
                { "8", "3000|3030" }   // FLAC
            };
            cat1.Add(3, cat1z3);

            result.Add(1, cat1);

            // Cat2 - Games
            var cat2 = new Dictionary<int, Dictionary<string, string>>();
            var cat2z0 = new Dictionary<string, string>
            {
                { "0", "4000|4050" },
                { "1", "4000|4030" },
                { "2", "TUX" },
                { "3", "PS" },
                { "4", "PS2" },
                { "5", "1000|1020" },
                { "6", "1000|1040" },
                { "7", "1000|1050" },
                { "8", "GBA" },
                { "9", "GC" },
                { "10", "1000|1010" },
                { "11", "1000|1030" },
                { "12", "1000|1080" },
                { "13", "4000|4040" },
                { "14", "4000|4040" },
                { "15", "4000|4040" },
                { "16", "3DS" },
                { "17", "PS4" },
                { "18", "XB1" }
            };
            cat2.Add(0, cat2z0);
            result.Add(2, cat2);

            // Cat3 - Applications
            var cat3 = new Dictionary<int, Dictionary<string, string>>();
            var cat3z0 = new Dictionary<string, string>
            {
                { "0", "4000|4020" },
                { "1", "4000|4030" },
                { "2", "TUX" },
                { "3", "OS/2" },
                { "4", "4000|4040" },
                { "5", "NAV" },
                { "6", "4000|4060" },
                { "7", "4000|4070" }
            };
            cat3.Add(0, cat3z0);
            result.Add(3, cat3);

            return result;
        }

        public static Dictionary<string, string> SpotBcat2nabcat()
        {
            return new Dictionary<string, string>
            {
                { "0", "" },
                { "1", "" },
                { "2", "" },
                { "3", "" },
                { "4", "5000" },
                { "5", "" },
                { "6", "5000" },
                { "7", "" },
                { "8", "" },
                { "9", "" },
                { "10", "" }
            };
        }
    }
}

