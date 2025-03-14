using System;
using System.Collections.Generic;
using System.Linq;

namespace SpotnetClient.SpotnetHelpers
{
    /* 
     * class SpotCategories
     */
    public static class SpotCategories
    {
        // private static $_namesTranslated = false;
        private static bool _namesTranslated = false;

        // public static $_head_categories =
        //     [0    => 'Image',
        //         1 => 'Sound',
        //         2 => 'Games',
        //         3 => 'Applications', ];
        public static Dictionary<int, string> _head_categories = new Dictionary<int, string>
        {
            { 0, "Image" },
            { 1, "Sound" },
            { 2, "Games" },
            { 3, "Applications" }
        };

        // public static $_headcat_subcat_mapping =
        //     [0    => 'd',
        //         1 => 'd',
        //         2 => 'c',
        //         3 => 'b', ];
        public static Dictionary<int, string> _headcat_subcat_mapping = new Dictionary<int, string>
        {
            { 0, "d" },
            { 1, "d" },
            { 2, "c" },
            { 3, "b" }
        };

        // public static $_subcat_descriptions =
        //     [0 => ['a'    => 'Format',
        //         'b'       => 'Source',
        //         'c'       => 'Language',
        //         'd'       => 'Genre',
        //         'z'       => 'Type', ],
        //         1 => ['a' => 'Format',
        //             'b'   => 'Source',
        //             'c'   => 'Bitrate',
        //             'd'   => 'Genre',
        //             'z'   => 'Type', ],
        //         2 => ['a' => 'Platform',
        //             'b'   => 'Format',
        //             'c'   => 'Genre',
        //             'z'   => 'Type', ],
        //         3 => ['a' => 'Platform',
        //             'b'   => 'Genre',
        //             'z'   => 'Type', ],
        //     ];
        public static Dictionary<int, Dictionary<string, string>> _subcat_descriptions = new Dictionary<int, Dictionary<string, string>>
        {
            { 0, new Dictionary<string, string> { { "a", "Format" }, { "b", "Source" }, { "c", "Language" }, { "d", "Genre" }, { "z", "Type" } } },
            { 1, new Dictionary<string, string> { { "a", "Format" }, { "b", "Source" }, { "c", "Bitrate" }, { "d", "Genre" }, { "z", "Type" } } },
            { 2, new Dictionary<string, string> { { "a", "Platform" }, { "b", "Format" }, { "c", "Genre" }, { "z", "Type" } } },
            { 3, new Dictionary<string, string> { { "a", "Platform" }, { "b", "Genre" }, { "z", "Type" } } }
        };

        // public static $_shortcat =
        //     [0 => [0    => 'DivX',
        //         1       => 'WMV',
        //         2       => 'MPG',
        //         3       => 'DVD5',
        //         4       => 'HD Oth',
        //         5       => 'ePub',
        //         6       => 'Blu-ray',
        //         7       => 'HD-DVD',
        //         8       => 'WMVHD',
        //         9       => 'x264HD',
        //         10      => 'DVD9',
        //         11      => 'PDF',
        //         12      => 'Bitmap',
        //         13      => 'Vector',
        //         14      => '3D',
        //         15      => 'UHD', ],
        //         1 => [0	=> 'MP3',
        //             1   => 'WMA',
        //             2   => 'WAV',
        //             3   => 'OGG',
        //             4   => 'EAC',
        //             5   => 'DTS',
        //             6   => 'AAC',
        //             7   => 'APE',
        //             8   => 'FLAC', ],
        //         2 => [0 => 'WIN',
        //             1   => 'MAC',
        //             2   => 'TUX',
        //             3   => 'PS',
        //             4   => 'PS2',
        //             5   => 'PSP',
        //             6   => 'XBX',
        //             7   => '360',
        //             8   => 'GBA',
        //             9   => 'GC',
        //             10  => 'NDS',
        //             11  => 'Wii',
        //             12  => 'PS3',
        //             13  => 'WinPh',
        //             14  => 'iOS',
        //             15  => 'Android',
        //             16  => '3DS',
        //             17  => 'PS4',
        //             18  => 'XB1',
        //         ],
        //         3 => [0 => 'WIN',
        //             1   => 'MAC',
        //             2   => 'TUX',
        //             3   => 'OS/2',
        //             4   => 'WinPh',
        //             5   => 'NAV',
        //             6   => 'iOS',
        //             7   => 'Android', ],
        //     ];
        public static Dictionary<int, Dictionary<int, string>> _shortcat = new Dictionary<int, Dictionary<int, string>>
        {
            { 0, new Dictionary<int, string>
                {
                    { 0, "DivX" },
                    { 1, "WMV" },
                    { 2, "MPG" },
                    { 3, "DVD5" },
                    { 4, "HD Oth" },
                    { 5, "ePub" },
                    { 6, "Blu-ray" },
                    { 7, "HD-DVD" },
                    { 8, "WMVHD" },
                    { 9, "x264HD" },
                    { 10, "DVD9" },
                    { 11, "PDF" },
                    { 12, "Bitmap" },
                    { 13, "Vector" },
                    { 14, "3D" },
                    { 15, "UHD" }
                }
            },
            { 1, new Dictionary<int, string>
                {
                    { 0, "MP3" },
                    { 1, "WMA" },
                    { 2, "WAV" },
                    { 3, "OGG" },
                    { 4, "EAC" },
                    { 5, "DTS" },
                    { 6, "AAC" },
                    { 7, "APE" },
                    { 8, "FLAC" }
                }
            },
            { 2, new Dictionary<int, string>
                {
                    { 0, "WIN" },
                    { 1, "MAC" },
                    { 2, "TUX" },
                    { 3, "PS" },
                    { 4, "PS2" },
                    { 5, "PSP" },
                    { 6, "XBX" },
                    { 7, "360" },
                    { 8, "GBA" },
                    { 9, "GC" },
                    { 10, "NDS" },
                    { 11, "Wii" },
                    { 12, "PS3" },
                    { 13, "WinPh" },
                    { 14, "iOS" },
                    { 15, "Android" },
                    { 16, "3DS" },
                    { 17, "PS4" },
                    { 18, "XB1" }
                }
            },
            { 3, new Dictionary<int, string>
                {
                    { 0, "WIN" },
                    { 1, "MAC" },
                    { 2, "TUX" },
                    { 3, "OS/2" },
                    { 4, "WinPh" },
                    { 5, "NAV" },
                    { 6, "iOS" },
                    { 7, "Android" }
                }
            }
        };

        /*
         * The structure of the categorynames is als follows:
         *
         *  [0] == Name
         *  [1] == To which 'type' (eg: Movie, Book, Erotica, etc) are they available for new selections
         *  [2] == To which 'type' *were* they available in the past
         *
         * 	We cannot call the gettxt routines directly on this structure, so do this later
         */
        public static Dictionary<int, Dictionary<string, Dictionary<object, object>>> _categories = new Dictionary<int, Dictionary<string, Dictionary<object, object>>>
        {
            { 0, new Dictionary<string, Dictionary<object, object>>
                {
                    { "a", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("DivX", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 1, new CategoryEntry("WMV", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 2, new CategoryEntry("MPG", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 3, new CategoryEntry("DVD5", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 4, new CategoryEntry("HD other", new string[]{}, new string[] { "z0", "z1", "z3" }) },
                            { 5, new CategoryEntry("ePub", new string[] { "z2" }, new string[] { "z2" }) },
                            { 6, new CategoryEntry("Blu-ray", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 7, new CategoryEntry("HD-DVD", new string[]{}, new string[] { "z0", "z1", "z3" }) },
                            { 8, new CategoryEntry("WMVHD", new string[]{}, new string[] { "z0", "z1", "z3" }) },
                            { 9, new CategoryEntry("x264", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 10, new CategoryEntry("DVD9", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 11, new CategoryEntry("PDF", new string[] { "z2" }, new string[] { "z2" }) },
                            { 12, new CategoryEntry("Bitmap", new string[] { "z4" }, new string[] { "z4" }) },
                            { 13, new CategoryEntry("Vector", new string[] { "z4" }, new string[] { "z4" }) },
                            { 14, new CategoryEntry("3D", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 15, new CategoryEntry("UHD", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                        }
                    },
                    { "b", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("CAM", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 1, new CategoryEntry("(S)VCD", new string[]{}, new string[] { "z0", "z1", "z3" }) },
                            { 2, new CategoryEntry("Promo", new string[]{}, new string[] { "z0", "z1", "z3" }) },
                            { 3, new CategoryEntry("Retail", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 4, new CategoryEntry("TV", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 5, new CategoryEntry("-", new string[]{}, new string[]{}) },
                            { 6, new CategoryEntry("Satellite", new string[]{}, new string[] { "z0", "z1", "z3" }) },
                            { 7, new CategoryEntry("R5", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 8, new CategoryEntry("Telecine", new string[]{}, new string[] { "z0", "z1", "z3" }) },
                            { 9, new CategoryEntry("Telesync", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 10, new CategoryEntry("Scan", new string[] { "z2" }, new string[] { "z2" }) },
                            { 11, new CategoryEntry("WEB-DL", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 12, new CategoryEntry("WEBRip", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 13, new CategoryEntry("HDRip", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                        }
                    },
                    { "c", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("No subtitles", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 1, new CategoryEntry("Dutch subtitles (external)", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 2, new CategoryEntry("Dutch subtitles (builtin)", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 3, new CategoryEntry("English subtitles (external)", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 4, new CategoryEntry("English subtitles (builtin)", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 5, new CategoryEntry("-", new string[]{}, new string[]{}) },
                            { 6, new CategoryEntry("Dutch subtitles (available)", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 7, new CategoryEntry("English subtitles (available)", new string[] { "z0", "z1", "z3" }, new string[] { "z0", "z1", "z3" }) },
                            { 8, new CategoryEntry("-", new string[]{}, new string[]{}) },
                            { 9, new CategoryEntry("-", new string[]{}, new string[]{}) },
                            { 10, new CategoryEntry("English audio/written", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 11, new CategoryEntry("Dutch audio/written", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 12, new CategoryEntry("German audio/written", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 13, new CategoryEntry("French audio/written", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 14, new CategoryEntry("Spanish audio/written", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 15, new CategoryEntry("Asian audio/written", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                        }
                    },
                    { "d", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("Action", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 1, new CategoryEntry("Adventure", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 2, new CategoryEntry("Animation", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 3, new CategoryEntry("Cabaret", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 4, new CategoryEntry("Comedy", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 5, new CategoryEntry("Crime", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 6, new CategoryEntry("Documentary", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 7, new CategoryEntry("Drama", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 8, new CategoryEntry("Family", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 9, new CategoryEntry("Fantasy", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 10, new CategoryEntry("Arthouse", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 11, new CategoryEntry("Television", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 12, new CategoryEntry("Horror", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 13, new CategoryEntry("Music", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 14, new CategoryEntry("Musical", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 15, new CategoryEntry("Mystery", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 16, new CategoryEntry("Romance", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 17, new CategoryEntry("Science Fiction", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 18, new CategoryEntry("Sport", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 19, new CategoryEntry("Short movie", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 20, new CategoryEntry("Thriller", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 21, new CategoryEntry("War", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 22, new CategoryEntry("Western", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 23, new CategoryEntry("Erotica (hetero)", new string[]{}, new string[] { "z3" }) },
                            { 24, new CategoryEntry("Erotica (gay male)", new string[]{}, new string[] { "z3" }) },
                            { 25, new CategoryEntry("Erotica (gay female)", new string[]{}, new string[] { "z3" }) },
                            { 26, new CategoryEntry("Erotica (bi)", new string[]{}, new string[] { "z3" }) },
                            { 27, new CategoryEntry("-", new string[]{}, new string[]{}) },
                            { 28, new CategoryEntry("Asian", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 29, new CategoryEntry("Anime", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 30, new CategoryEntry("Cover", new string[] { "z2" }, new string[] { "z2" }) },
                            { 31, new CategoryEntry("Comicbook", new string[] { "z2" }, new string[] { "z2" }) },
                            { 32, new CategoryEntry("Cartoons", new string[] { "z2" }, new string[] { "z2" }) },
                            { 33, new CategoryEntry("Youth", new string[] { "z2" }, new string[] { "z2" }) },
                            { 34, new CategoryEntry("Business", new string[] { "z2" }, new string[] { "z2" }) },
                            { 35, new CategoryEntry("Computer", new string[] { "z2" }, new string[] { "z2" }) },
                            { 36, new CategoryEntry("Hobby", new string[] { "z2" }, new string[] { "z2" }) },
                            { 37, new CategoryEntry("Cooking", new string[] { "z2" }, new string[] { "z2" }) },
                            { 38, new CategoryEntry("Handwork", new string[] { "z2" }, new string[] { "z2" }) },
                            { 39, new CategoryEntry("Craftwork", new string[] { "z2" }, new string[] { "z2" }) },
                            { 40, new CategoryEntry("Health", new string[] { "z2" }, new string[] { "z2" }) },
                            { 41, new CategoryEntry("History", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 42, new CategoryEntry("Psychology", new string[] { "z2" }, new string[] { "z2" }) },
                            { 43, new CategoryEntry("Newspaper", new string[] { "z2" }, new string[] { "z2" }) },
                            { 44, new CategoryEntry("Magazine", new string[] { "z2" }, new string[] { "z2" }) },
                            { 45, new CategoryEntry("Science", new string[] { "z2" }, new string[] { "z2" }) },
                            { 46, new CategoryEntry("Female", new string[] { "z2" }, new string[] { "z2" }) },
                            { 47, new CategoryEntry("Religion", new string[] { "z2" }, new string[] { "z2" }) },
                            { 48, new CategoryEntry("Roman", new string[] { "z2" }, new string[] { "z2" }) },
                            { 49, new CategoryEntry("Biography", new string[] { "z2" }, new string[] { "z2" }) },
                            { 50, new CategoryEntry("Detective", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 51, new CategoryEntry("Animals", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 52, new CategoryEntry("Humor", new string[] { "z0", "z1", "z2" }, new string[] { "z0", "z1", "z2" }) },
                            { 53, new CategoryEntry("Travel", new string[] { "z2" }, new string[] { "z2" }) },
                            { 54, new CategoryEntry("True story", new string[] { "z0", "z1" }, new string[] { "z0", "z1" }) },
                            { 55, new CategoryEntry("Non-fiction", new string[] { "z2" }, new string[] { "z2" }) },
                            { 56, new CategoryEntry("Politics", new string[]{}, new string[]{}) },
                            { 57, new CategoryEntry("Poetry", new string[] { "z2" }, new string[] { "z2" }) },
                            { 58, new CategoryEntry("Fairy tale", new string[] { "z2" }, new string[] { "z2" }) },
                            { 59, new CategoryEntry("Technical", new string[] { "z2" }, new string[] { "z2" }) },
                            { 60, new CategoryEntry("Art", new string[] { "z2" }, new string[] { "z2" }) },
                            { 72, new CategoryEntry("Bi", new string[] { "z3" }, new string[] { "z3" }) },
                            { 73, new CategoryEntry("Lesbian", new string[] { "z3" }, new string[] { "z3" }) },
                            { 74, new CategoryEntry("Homo", new string[] { "z3" }, new string[] { "z3" }) },
                            { 75, new CategoryEntry("Hetero", new string[] { "z3" }, new string[] { "z3" }) },
                            { 76, new CategoryEntry("Amature", new string[] { "z3" }, new string[] { "z3" }) },
                            { 77, new CategoryEntry("Group", new string[] { "z3" }, new string[] { "z3" }) },
                            { 78, new CategoryEntry("POV", new string[] { "z3" }, new string[] { "z3" }) },
                            { 79, new CategoryEntry("Solo", new string[] { "z3" }, new string[] { "z3" }) },
                            { 80, new CategoryEntry("Young", new string[] { "z3" }, new string[] { "z3" }) },
                            { 81, new CategoryEntry("Soft", new string[] { "z3" }, new string[] { "z3" }) },
                            { 82, new CategoryEntry("Fetish", new string[] { "z3" }, new string[] { "z3" }) },
                            { 83, new CategoryEntry("Old", new string[] { "z3" }, new string[] { "z3" }) },
                            { 84, new CategoryEntry("Fat", new string[] { "z3" }, new string[] { "z3" }) },
                            { 85, new CategoryEntry("SM", new string[] { "z3" }, new string[] { "z3" }) },
                            { 86, new CategoryEntry("Rough", new string[] { "z3" }, new string[] { "z3" }) },
                            { 87, new CategoryEntry("Dark", new string[] { "z3" }, new string[] { "z3" }) },
                            { 88, new CategoryEntry("Hentai", new string[] { "z3" }, new string[] { "z3" }) },
                            { 89, new CategoryEntry("Outside", new string[] { "z3" }, new string[] { "z3" }) },
                        }
                    },
                    { "z", new Dictionary<object, object>
                        {
                            { 0, "Movie" },
                            { 1, "Series" },
                            { 2, "Book" },
                            { 3, "Erotica" },
                            { 4, "Picture" },
                        }
                    }
                }
            },
            { 1, new Dictionary<string, Dictionary<object, object>>
                {
                    { "a", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("MP3", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 1, new CategoryEntry("WMA", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 2, new CategoryEntry("WAV", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 3, new CategoryEntry("OGG", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 4, new CategoryEntry("EAC", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 5, new CategoryEntry("DTS", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 6, new CategoryEntry("AAC", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 7, new CategoryEntry("APE", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 8, new CategoryEntry("FLAC", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                        }
                    },
                    { "b", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("CD", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 1, new CategoryEntry("Radio", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 2, new CategoryEntry("Compilation", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 3, new CategoryEntry("DVD", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 4, new CategoryEntry("Other", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 5, new CategoryEntry("Vinyl", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 6, new CategoryEntry("Stream", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                        }
                    },
                    { "c", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("Variable", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 1, new CategoryEntry("< 96kbit", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 2, new CategoryEntry("96kbit", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 3, new CategoryEntry("128kbit", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 4, new CategoryEntry("160kbit", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 5, new CategoryEntry("192kbit", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 6, new CategoryEntry("256kbit", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 7, new CategoryEntry("320kbit", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 8, new CategoryEntry("Lossless", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 9, new CategoryEntry("Other", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                        }
                    },
                    { "d", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("Blues", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 1, new CategoryEntry("Compilation", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 2, new CategoryEntry("Cabaret", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 3, new CategoryEntry("Dance", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 4, new CategoryEntry("Diverse", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 5, new CategoryEntry("Hardcore", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 6, new CategoryEntry("World", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 7, new CategoryEntry("Jazz", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 8, new CategoryEntry("Youth", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 9, new CategoryEntry("Classical", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 10, new CategoryEntry("Kleinkunst", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 11, new CategoryEntry("Dutch", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 12, new CategoryEntry("New Age", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 13, new CategoryEntry("Pop", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 14, new CategoryEntry("RnB", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 15, new CategoryEntry("Hiphop", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 16, new CategoryEntry("Reggae", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 17, new CategoryEntry("Religious", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 18, new CategoryEntry("Rock", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 19, new CategoryEntry("Soundtracks", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 20, new CategoryEntry("Other", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 21, new CategoryEntry("Hardstyle", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 22, new CategoryEntry("Asian", new string[]{}, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 23, new CategoryEntry("Disco", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 24, new CategoryEntry("Classics", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 25, new CategoryEntry("Metal", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 26, new CategoryEntry("Country", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 27, new CategoryEntry("Dubstep", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 28, new CategoryEntry("Nederhop", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 29, new CategoryEntry("DnB", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 30, new CategoryEntry("Electro", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 31, new CategoryEntry("Folk", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 32, new CategoryEntry("Soul", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 33, new CategoryEntry("Trance", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 34, new CategoryEntry("Balkan", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 35, new CategoryEntry("Techno", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 36, new CategoryEntry("Ambient", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 37, new CategoryEntry("Latin", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                            { 38, new CategoryEntry("Live", new string[] { "z0", "z1", "z2", "z3" }, new string[] { "z0", "z1", "z2", "z3" }) },
                        }
                    },
                    { "z", new Dictionary<object, object>
                        {
                            { 0, "Album" },
                            { 1, "Liveset" },
                            { 2, "Podcast" },
                            { 3, "Audiobook" },
                        }
                    }
                }
            },
            { 2, new Dictionary<string, Dictionary<object, object>>
                {
                    { "a", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("Windows", new string[] { "zz" }, new string[] { "zz" }) },
                            { 1, new CategoryEntry("Macintosh", new string[] { "zz" }, new string[] { "zz" }) },
                            { 2, new CategoryEntry("Linux", new string[] { "zz" }, new string[] { "zz" }) },
                            { 3, new CategoryEntry("Playstation", new string[] { "zz" }, new string[] { "zz" }) },
                            { 4, new CategoryEntry("Playstation 2", new string[] { "zz" }, new string[] { "zz" }) },
                            { 5, new CategoryEntry("PSP", new string[] { "zz" }, new string[] { "zz" }) },
                            { 6, new CategoryEntry("Xbox", new string[] { "zz" }, new string[] { "zz" }) },
                            { 7, new CategoryEntry("Xbox 360", new string[] { "zz" }, new string[] { "zz" }) },
                            { 8, new CategoryEntry("Gameboy Advance", new string[] { "zz" }, new string[] { "zz" }) },
                            { 9, new CategoryEntry("Gamecube", new string[] { "zz" }, new string[] { "zz" }) },
                            { 10, new CategoryEntry("Nintendo DS", new string[] { "zz" }, new string[] { "zz" }) },
                            { 11, new CategoryEntry("Nintento Wii", new string[] { "zz" }, new string[] { "zz" }) },
                            { 12, new CategoryEntry("Playstation 3", new string[] { "zz" }, new string[] { "zz" }) },
                            { 13, new CategoryEntry("Windows Phone", new string[] { "zz" }, new string[] { "zz" }) },
                            { 14, new CategoryEntry("iOS", new string[] { "zz" }, new string[] { "zz" }) },
                            { 15, new CategoryEntry("Android", new string[] { "zz" }, new string[] { "zz" }) },
                            { 16, new CategoryEntry("Nintendo 3DS", new string[] { "zz" }, new string[] { "zz" }) },
                            { 17, new CategoryEntry("Playstation 4", new string[] { "zz" }, new string[] { "zz" }) },
                            { 18, new CategoryEntry("XBox 1", new string[] { "zz" }, new string[] { "zz" }) },
                        }
                    },
                    { "b", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("ISO", new string[]{}, new string[] { "zz" }) },
                            { 1, new CategoryEntry("Rip", new string[] { "zz" }, new string[] { "zz" }) },
                            { 2, new CategoryEntry("Retail", new string[] { "zz" }, new string[] { "zz" }) },
                            { 3, new CategoryEntry("DLC", new string[] { "zz" }, new string[] { "zz" }) },
                            { 4, new CategoryEntry("", new string[]{}, new string[]{}) },
                            { 5, new CategoryEntry("Patch", new string[] { "zz" }, new string[] { "zz" }) },
                            { 6, new CategoryEntry("Crack", new string[] { "zz" }, new string[] { "zz" }) },
                        }
                    },
                    { "c", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("Action", new string[] { "zz" }, new string[] { "zz" }) },
                            { 1, new CategoryEntry("Adventure", new string[] { "zz" }, new string[] { "zz" }) },
                            { 2, new CategoryEntry("Strategy", new string[] { "zz" }, new string[] { "zz" }) },
                            { 3, new CategoryEntry("Roleplaying", new string[] { "zz" }, new string[] { "zz" }) },
                            { 4, new CategoryEntry("Simulation", new string[] { "zz" }, new string[] { "zz" }) },
                            { 5, new CategoryEntry("Race", new string[] { "zz" }, new string[] { "zz" }) },
                            { 6, new CategoryEntry("Flying", new string[] { "zz" }, new string[] { "zz" }) },
                            { 7, new CategoryEntry("Shooter", new string[] { "zz" }, new string[] { "zz" }) },
                            { 8, new CategoryEntry("Platform", new string[] { "zz" }, new string[] { "zz" }) },
                            { 9, new CategoryEntry("Sport", new string[] { "zz" }, new string[] { "zz" }) },
                            { 10, new CategoryEntry("Child/youth", new string[] { "zz" }, new string[] { "zz" }) },
                            { 11, new CategoryEntry("Puzzle", new string[] { "zz" }, new string[] { "zz" }) },
                            { 12, new CategoryEntry("Other", new string[]{}, new string[] { "zz" }) },
                            { 13, new CategoryEntry("Boardgame", new string[] { "zz" }, new string[] { "zz" }) },
                            { 14, new CategoryEntry("Cards", new string[] { "zz" }, new string[] { "zz" }) },
                            { 15, new CategoryEntry("Education", new string[] { "zz" }, new string[] { "zz" }) },
                            { 16, new CategoryEntry("Music", new string[] { "zz" }, new string[] { "zz" }) },
                            { 17, new CategoryEntry("Family", new string[] { "zz" }, new string[] { "zz" }) },
                        }
                    },
                    { "z", new Dictionary<object, object>
                        {
                            { "z", "everything" }
                        }
                    }
                }
            },
            { 3, new Dictionary<string, Dictionary<object, object>>
                {
                    { "a", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("Windows", new string[] { "zz" }, new string[] { "zz" }) },
                            { 1, new CategoryEntry("Macintosh", new string[] { "zz" }, new string[] { "zz" }) },
                            { 2, new CategoryEntry("Linux", new string[] { "zz" }, new string[] { "zz" }) },
                            { 3, new CategoryEntry("OS/2", new string[] { "zz" }, new string[] { "zz" }) },
                            { 4, new CategoryEntry("Windows Phone", new string[] { "zz" }, new string[] { "zz" }) },
                            { 5, new CategoryEntry("Navigation systems", new string[] { "zz" }, new string[] { "zz" }) },
                            { 6, new CategoryEntry("iOS", new string[] { "zz" }, new string[] { "zz" }) },
                            { 7, new CategoryEntry("Android", new string[] { "zz" }, new string[] { "zz" }) },
                        }
                    },
                    { "b", new Dictionary<object, object>
                        {
                            { 0, new CategoryEntry("Audio", new string[] { "zz" }, new string[] { "zz" }) },
                            { 1, new CategoryEntry("Video", new string[] { "zz" }, new string[] { "zz" }) },
                            { 2, new CategoryEntry("Graphics", new string[] { "zz" }, new string[] { "zz" }) },
                            { 3, new CategoryEntry("CD/DVD Tools", new string[]{}, new string[] { "zz" }) },
                            { 4, new CategoryEntry("Media players", new string[]{}, new string[] { "zz" }) },
                            { 5, new CategoryEntry("Rippers & Encoders", new string[]{}, new string[]{}) },
                            { 6, new CategoryEntry("Plugins", new string[]{}, new string[] { "zz" }) },
                            { 7, new CategoryEntry("Database tools", new string[]{}, new string[] { "zz" }) },
                            { 8, new CategoryEntry("Email software", new string[]{}, new string[] { "zz" }) },
                            { 9, new CategoryEntry("Photo", new string[]{}, new string[] { "zz" }) },
                            { 10, new CategoryEntry("Screensavers", new string[]{}, new string[] { "zz" }) },
                            { 11, new CategoryEntry("Skin software", new string[]{}, new string[] { "zz" }) },
                            { 12, new CategoryEntry("Drivers", new string[]{}, new string[] { "zz" }) },
                            { 13, new CategoryEntry("Browsers", new string[]{}, new string[] { "zz" }) },
                            { 14, new CategoryEntry("Download managers", new string[]{}, new string[]{}) },
                            { 15, new CategoryEntry("Download", new string[] { "zz" }, new string[] { "zz" }) },
                            { 16, new CategoryEntry("Usenet software", new string[]{}, new string[] { "zz" }) },
                            { 17, new CategoryEntry("RSS Readers", new string[]{}, new string[] { "zz" }) },
                            { 18, new CategoryEntry("FTP software", new string[]{}, new string[] { "zz" }) },
                            { 19, new CategoryEntry("Firewalls", new string[]{}, new string[] { "zz" }) },
                            { 20, new CategoryEntry("Antivirus software", new string[]{}, new string[] { "zz" }) },
                            { 21, new CategoryEntry("Antispyware software", new string[]{}, new string[] { "zz" }) },
                            { 22, new CategoryEntry("Optimization software", new string[]{}, new string[] { "zz" }) },
                            { 23, new CategoryEntry("Security software", new string[] { "zz" }, new string[] { "zz" }) },
                            { 24, new CategoryEntry("System software", new string[] { "zz" }, new string[] { "zz" }) },
                            { 25, new CategoryEntry("Other", new string[]{}, new string[] { "zz" }) },
                            { 26, new CategoryEntry("Educational", new string[]{}, new string[] { "zz" }) },
                            { 27, new CategoryEntry("Office", new string[]{}, new string[] { "zz" }) },
                            { 28, new CategoryEntry("Internet", new string[]{}, new string[] { "zz" }) },
                            { 29, new CategoryEntry("Communication", new string[]{}, new string[] { "zz" }) },
                            { 30, new CategoryEntry("Development", new string[]{}, new string[] { "zz" }) },
                            { 31, new CategoryEntry("Spotnet", new string[] { "zz" }, new string[] { "zz" }) },
                        }
                    },
                    { "z", new Dictionary<object, object>
                        {
                            { "z", "everything" }
                        }
                    }
                }
            }
        };

        public static string Cat2Desc(int hcat, string cat)
        {
            string[] catList = cat.Split('|');
            cat = catList[0];

            if (string.IsNullOrEmpty(cat) || cat.Length == 0)
            {
                return "";
            } // if

            char type = cat[0];
            string nr = cat.Substring(1);

            if (!_categories.ContainsKey(hcat) ||
                !_categories[hcat].ContainsKey(type.ToString()) ||
                !_categories[hcat][type.ToString()].ContainsKey(int.Parse(nr)) ||
                !(_categories[hcat][type.ToString()][int.Parse(nr)] is CategoryEntry || type == 'z'))
            {
                return "-";
            }
            else
            {
                if (type != 'z')
                {
                    return ((CategoryEntry)_categories[hcat][type.ToString()][int.Parse(nr)]).Name;
                }
                else
                {
                    return _categories[hcat][type.ToString()][int.Parse(nr)] as string;
                } // else
            } // if
        }

        public static string Cat2ShortDesc(int hcat, string cat)
        {
            string[] catList = cat.Split('|');
            cat = catList[0];

            if (string.IsNullOrEmpty(cat) || cat.Length == 0)
            {
                return "";
            } // if

            string nr = cat.Substring(1);

            if (!_shortcat.ContainsKey(hcat) || !_shortcat[hcat].ContainsKey(int.Parse(nr)))
            {
                return "-";
            }
            else
            {
                return _shortcat[hcat][int.Parse(nr)];
            } // if
        }

        public static string SubcatDescription(int hcat, string ch)
        {
            if ((_subcat_descriptions.ContainsKey(hcat)) && (_subcat_descriptions[hcat].ContainsKey(ch)))
            {
                return _subcat_descriptions[hcat][ch];
            }
            else
            {
                return "-";
            } // else
        }

        // func SubcatDescription

        public static string SubcatNumberFromHeadcat(int hcat)
        {
            if (_headcat_subcat_mapping.ContainsKey(hcat))
            {
                return _headcat_subcat_mapping[hcat];
            }
            else
            {
                return "-";
            } // else
        }

        // SubcatNumberFromHeadcat

        public static string HeadCat2Desc(int cat)
        {
            if (_head_categories.ContainsKey(cat))
            {
                return _head_categories[cat];
            }
            else
            {
                return "-";
            } // else
        }

        // func. Cat2Desc

        public static string createSubcatZ(int hcat, string subcats)
        {
            // z-categorieen gelden tot nu toe enkel voor films en muziek
            if (hcat != 0 && hcat != 1)
            {
                return "";
            } // if

            string[] genreSubcatList = subcats.Split('|');
            string subcatz = "";

            foreach (var subCatVal in genreSubcatList)
            {
                if (string.IsNullOrEmpty(subCatVal))
                {
                    continue;
                } // if

                if (hcat == 0)
                {
                    // 'Erotiek'
                    if (("d23|d24|d25|d26|d72|d73|d74|d75|d76|d77|d78|d79|d80|d81|d82|d83|d84|d85|d86|d87|d88|d89|")
                        .IndexOf(subCatVal + "|", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        subcatz = "z3|";
                    }
                    else if (("b4|d11|").IndexOf(subCatVal + "|", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        // Series
                        subcatz = "z1|";
                    }
                    else if (("a5|a11|").IndexOf(subCatVal + "|", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        // Boeken
                        subcatz = "z2|";
                    }
                    else if (("a12|a13|").IndexOf(subCatVal + "|", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        // Plaatjes
                        subcatz = "z4|";
                    }
                    else if (string.IsNullOrEmpty(subcatz))
                    {
                        // default, film
                        subcatz = "z0|";
                    } // else
                }
                else if (hcat == 1)
                {
                    subcatz = "z0|";
                    break;
                } // if muziek
            } // foreach

            return subcatz;
        }

        // createSubcatZ

        public static string mapDeprecatedGenreSubCategories(int hcat, string subcatlist, string subcatz)
        {
            // image
            if (hcat == 0)
            {
                // map deprecated adult categories
                if (subcatz == "z3|")
                {
                    subcatlist = replaceSubCategory(subcatlist, "d23|", "d75|");
                    subcatlist = replaceSubCategory(subcatlist, "d24|", "d74|");
                    subcatlist = replaceSubCategory(subcatlist, "d25|", "d73|");
                    subcatlist = replaceSubCategory(subcatlist, "d26|", "d72|");
                } // if
            }

            return subcatlist;
        }

        public static string mapLanguageSubCategories(int hcat, string subcatlist, string subcatz)
        {
            // image
            if (hcat == 0)
            {
                // map book language subcategories to audio/written instead of subtitle
                // this is a deviation from https://github.com/Spotnet/Spotnet/wiki/Category-Codes
                // https://github.com/spotweb/spotweb/issues/1724
                if (subcatz == "z2|")
                {
                    // Dutch: C2/C3/C7 => C12 Note that Spotweb internally works with a 0-based index
                    subcatlist = replaceSubCategory(subcatlist, "c1|", "c11|");
                    subcatlist = replaceSubCategory(subcatlist, "c2|", "c11|");
                    subcatlist = replaceSubCategory(subcatlist, "c6|", "c11|");
                    // English: C4/C5/C8 => C11
                    subcatlist = replaceSubCategory(subcatlist, "c3|", "c10|");
                    subcatlist = replaceSubCategory(subcatlist, "c4|", "c10|");
                    subcatlist = replaceSubCategory(subcatlist, "c7|", "c10|");
                } // if
            }

            return subcatlist;
        }

        private static string replaceSubCategory(string subcatlist, string oldsubcat, string newsubcat)
        {
            if (subcatlist.IndexOf(oldsubcat, StringComparison.OrdinalIgnoreCase) != -1)
            {
                // prevent new subcategory being listed twice
                // if the new subcategory already exists, we replace the old subcategory with nothing
                if (subcatlist.IndexOf(newsubcat, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    subcatlist = subcatlist.Replace(oldsubcat, "", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    subcatlist = subcatlist.Replace(oldsubcat, newsubcat, StringComparison.OrdinalIgnoreCase);
                }
            }

            return subcatlist;
        }

        public static void startTranslation()
        {
            /*
             * Make sure we only translate once
             */
            if (_namesTranslated)
            {
                return;
            } // if
            _namesTranslated = true;

            // Translate the head categories
            foreach (var key in _head_categories.Keys.ToList())
            {
                _head_categories[key] = Translate(_head_categories[key]);

                // Translate the subcat descriptions
                if (_subcat_descriptions.ContainsKey(key))
                {
                    foreach (var subkey in _subcat_descriptions[key].Keys.ToList())
                    {
                        _subcat_descriptions[key][subkey] = Translate(_subcat_descriptions[key][subkey]);
                    } // foreach
                }

                // Translate the shortcat descriptions
                if (_shortcat.ContainsKey(key))
                {
                    foreach (var subkey in _shortcat[key].Keys.ToList())
                    {
                        _shortcat[key][subkey] = Translate(_shortcat[key][subkey]);
                    } // foreach
                }

                // and translate the actual categories
                if (_categories.ContainsKey(key))
                {
                    foreach (var subkey in _categories[key].Keys.ToList())
                    {
                        foreach (var subsubkey in _categories[key][subkey].Keys.ToList())
                        {
                            var subsubvalue = _categories[key][subkey][subsubkey];
                            if (subsubvalue is CategoryEntry ce)
                            {
                                ce.Name = Translate(ce.Name);
                            }
                            else if (subsubvalue is string s)
                            {
                                _categories[key][subkey][subsubkey] = Translate(s);
                            } // else
                        } // foreach
                    } // foreach
                } // foreach
            } // foreach
        }

        // startTranslation

        private static string Translate(string input)
        {
            // Placeholder: replace with your actual localization routine.
            return input;
        }
    }

    public class CategoryEntry
    {
        public string Name { get; set; }
        public string[] NewSelection { get; set; }
        public string[] OldSelection { get; set; }

        public CategoryEntry(string name, string[] newSelection, string[] oldSelection)
        {
            Name = name;
            NewSelection = newSelection;
            OldSelection = oldSelection;
        }
    }
}
