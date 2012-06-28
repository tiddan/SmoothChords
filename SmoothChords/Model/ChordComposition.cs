using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PianoLibrary;
using System.Collections;

namespace SmoothChords.Model
{
    public class ChordComposition
    {
        public string Tonic { get; set; }
        public string Mode { get; set; }
        public string Quality { get; set; }
        public string Suspension { get; set; }
        public string Extention { get; set; }

        private int _cursorPos;

        public static List<string> ValidTonics = new List<string>
        {
            "C","C#","D","D#","E","F","F#","G","G#","A","A#","H"
        };

        public static List<string> ValidQualities = new List<string>
        {
            "maj", "aug", "dim",null
        };

        public static List<int> ValidExtentions = new List<int>
        {
            3,5,6,7,9,11,13
        };

        public bool IsValidNotation()
        {

            if (!ValidTonics.Contains(Tonic)) return false;

            if (Mode != null && Mode != "m") return false;

            if (!ValidQualities.Contains(Quality)) return false;

            if (Suspension != null && Suspension != "sus4" && Suspension != "sus2") return false;


            List<int> extList;
            if (!IsValidExtention(Extention, out extList)) return false;

            return true;
        }

        public static ChordComposition DecomposeChord(string chord)
        {
            ChordComposition comp = new ChordComposition();
            comp.CalculateTonic(chord);
            comp.CalculateMode(chord);

            if (chord.Contains("sus"))
            {
                comp.CalculateSuspension(chord);
            }
            else
            {
                comp.CalculateQuality(chord);
            }

            comp.CalculateExtentions(chord);

            return comp;
        }

        public static ObservableCollection<int> ComposeChord(string chord)
        {
            Hashtable lefthand = new Hashtable();
            Hashtable righthand = new Hashtable();

            ChordComposition comp = ChordComposition.DecomposeChord(chord);

            if (comp.IsValidNotation())
            {
                int baseLineOffset = (2 * 12);
                int chordOffset = (3 * 12);
                
                Hashtable ModifiedKeys = new Hashtable();

                PianoKey.KeyTypes tonicKey;
                if (Enum.TryParse<PianoKey.KeyTypes>(comp.Tonic.Replace("#", "Sharp"), out tonicKey))
                {
                    int tonicOffset = Piano.GetOffset(tonicKey);
                    
                    // Add base "I"
                    lefthand.Add("1", baseLineOffset + tonicOffset);

                    // Add chord-note "I"
                    righthand.Add("1", chordOffset + tonicOffset);

                    // Add chord-note "III"
                    righthand.Add("3", chordOffset + tonicOffset + Piano.OFFSET_3RD);

                    // Add chord-note "V"
                    righthand.Add("5", chordOffset + tonicOffset + Piano.OFFSET_5TH);

                    // Add Mode
                    if (comp.Mode == "m")
                    {
                        if (righthand.ContainsKey("3"))
                        {
                            righthand["3"] = (int)righthand["3"] - 1;
                            ModifiedKeys.Add("3", -1);
                        }
                    }

                    // Add Quality
                    if (comp.Quality != null)
                    {
                        if (comp.Quality == "maj")
                        {
                            righthand["7"] = chordOffset + tonicOffset + Piano.OFFSET_7TH + 1;
                            ModifiedKeys.Add("7", 0);
                        }
                        else if (comp.Quality == "aug")
                        {
                            righthand["5"] = (int)righthand["5"] + 1;
                            ModifiedKeys["5"] = +1;
                        }
                        else if (comp.Quality == "dim")
                        {
                            if(!ModifiedKeys.ContainsKey("3"))
                            {
                                righthand["3"] = (int)righthand["3"] - 1;
                                ModifiedKeys["3"] = -1;
                            }
                            righthand["5"] = (int)righthand["5"] - 1;
                            ModifiedKeys["5"] = -1;
                        }
                    }

                    // Add Suspension
                    if (comp.Suspension != null)
                    {
                        if (!ModifiedKeys.ContainsKey("3"))
                        {
                            if (comp.Suspension[3] == '4')
                            {
                                righthand["3"] = (int)righthand["3"] + 1;
                                ModifiedKeys["3"] = +1;
                            }
                            else
                            {
                                righthand["3"] = (int)righthand["3"] - 2;
                                ModifiedKeys["3"] = -2;
                            }
                        }
                    }

                    // Add Extentions
                    if (comp.Extention != null)
                    {
                        List<int> extList;
                        comp.IsValidExtention(comp.Extention, out extList);

                        foreach (int ext in extList)
                        {
                            
                            int modifier = 0;
                            int index = comp.Extention.IndexOf(ext.ToString());
                            if (index> 0)
                            {
                                string token = comp.Extention.ElementAt(index - 1).ToString();
                                if (token == "+")
                                {
                                    modifier++;
                                }
                                else if (token == "-")
                                {
                                    modifier--;
                                }
                            }

                            switch (ext)
                            {
                                case 5:
                                    {
                                        righthand["5"] = (int)righthand["5"] + modifier;
                                        break;
                                    }
                                case 7:
                                    {
                                        if(!ModifiedKeys.ContainsKey("7"))
                                        {
                                            righthand["7"] = chordOffset + tonicOffset + Piano.OFFSET_7TH + modifier;
                                            ModifiedKeys["7"] = 0;
                                        }
                                        break;
                                    }
                                case 9:
                                    {
                                        if (!ModifiedKeys.ContainsKey("9"))
                                        {
                                            righthand["9"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_9TH + modifier;
                                            ModifiedKeys["9"] = 0;
                                        }
                                        break;
                                    }
                                case 11:
                                    {
                                        if (!ModifiedKeys.ContainsKey("9"))
                                        {
                                            righthand["9"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_9TH;
                                            ModifiedKeys["9"] = 0;
                                        }
                                        if (!ModifiedKeys.ContainsKey("11"))
                                        {
                                            righthand["11"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_11TH + modifier;
                                            ModifiedKeys["9"] = 0;
                                        }
                                        break;
                                    }
                                case 13:
                                    {
                                        if (!ModifiedKeys.ContainsKey("9"))
                                        {
                                            righthand["9"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_9TH;
                                            ModifiedKeys["9"] = 0;
                                        }
                                        if (!ModifiedKeys.ContainsKey("11"))
                                        {
                                            righthand["11"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_11TH;
                                            ModifiedKeys["11"] = 0;
                                        }
                                        if (!ModifiedKeys.ContainsKey("13"))
                                        {
                                            righthand["13"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_13TH + modifier;
                                            ModifiedKeys["13"] = 0;
                                        }
                                        break;
                                    }
                                   
                            }


                            
                        }
                    }
                }
            }
            else
            {
                return new ObservableCollection<int>();
            }

            ObservableCollection<int> notes = new ObservableCollection<int>();
            foreach (int value in lefthand.Values) notes.Add(value);
            foreach (int value in righthand.Values) notes.Add(value);
            return notes;
        }

        private bool IsValidExtention(string extention, out List<int> extensionList)
        {
            extensionList = new List<int>();

            if (extention == null) return true;

            string mod = "";
            string ext = "";
            int i = 0;

            while (i < extention.Count())
            {
                char c = extention[i];

                if (c == '+' || c == '-')
                {
                    mod = "" + c;
                }
                else
                {
                    ext += c;

                    int e;
                    if (Int32.TryParse(ext, out e))
                    {
                        if (ValidExtentions.Contains(e))
                        {
                            if (!extensionList.Contains(e))
                            {
                                extensionList.Add(e);
                                ext = "";
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                i++;
            }

            if (ext != "")
            {
                return false;
            }

            return true;

        }

        private void CalculateTonic(string chord)
        {
            string tonic = "";
            var a = chord[0].ToString();
            _cursorPos++;
            tonic += a;
            if (chord.Count() > 1)
            {
                var b = chord[1].ToString();
                {
                    if (b == "#" || b.ToLower() == "b")
                    {
                        tonic += b;
                        _cursorPos++;
                    }
                }
            }

            Tonic = tonic;
        }

        private void CalculateMode(string chord)
        {
            if (chord.Count() >= _cursorPos + 1)
            {
                if (chord[_cursorPos].ToString() != "m")
                {
                    return;
                }
                else
                {
                    if (chord.Count() > (_cursorPos + 1))
                    {
                        string nextChar = chord[_cursorPos + 1].ToString();
                        if (nextChar != "a")
                        {
                            Mode = "m";
                            _cursorPos++;
                        }
                    }
                    else
                    {
                        Mode = "m";
                        _cursorPos++;
                    }
                }
            }
        }

        private void CalculateSuspension(string chord)
        {
            if (chord.Count() >= _cursorPos + 3)
            {
                if (chord.Substring(_cursorPos, 3) == "sus")
                {
                    string susType = "4";
                    if (chord.Count() >= _cursorPos + 4)
                    {
                        string next = chord.Substring(_cursorPos + 3, 1);
                        if (next == "4" || next == "2")
                        {
                            susType = next;
                            _cursorPos++;
                        }
                    }
                    _cursorPos += 3;
                    Suspension = "sus" + susType;
                }
            }
        }

        private void CalculateQuality(string chord)
        {
            if (chord.Count() >= _cursorPos + 3)
            {
                string q = chord.Substring(_cursorPos, 3).ToLower();
                if (q == "maj" || q == "aug" || q == "dim")
                {
                    Quality = q;
                    _cursorPos += 3;
                }
            }
        }

        private void CalculateExtentions(string chord)
        {
            int remaining = (chord.Count()) - _cursorPos;
            string remainingString = chord.Substring(_cursorPos, remaining);

            if (remainingString.Count() > 0)
            {
                Extention = remainingString;
            }
        }
    }
}

