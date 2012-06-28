using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PianoLibrary.Model.Chords
{
    public class ChordDefinition
    {
        public string Chord { get; set; }

        public string Tonic { get; set; }
        public Modes Mode { get; set; }
        public Qualities Quality { get; set; }
        public SuspentionModes Suspension { get; set; }
        public string Extention { get; set; }

        private int _cursorPos;

        public enum Modes { Major, Minor }

        public enum Qualities { none, dim, maj, aug }

        public enum TransposeType { Up, Down }

        public enum SuspentionModes { none, sus2, sus4 }

        public static List<string> ValidTonicBases = new List<string>
        {
            "C","D","E","F","G","A","B"
        };

        public static List<string> ValidQualities = new List<string>
        {
            "maj", "aug", "dim",null
        };

        public static List<int> ValidExtentions = new List<int>
        {
            3,5,6,7,9,11,13
        };

        public void Transpose(TransposeType type)
        {
            PianoLibrary.PianoKey.KeyTypes keyType;
            if(Enum.TryParse<PianoLibrary.PianoKey.KeyTypes>(Chord,out keyType))
            {
                if (type == TransposeType.Up)
                {
                    keyType = PianoKey.NextKey(keyType);
                }
                else
                {
                    keyType = PianoKey.PrevKey(keyType);
                }
            }
        }

        public bool IsValidNotation()
        {
            if (Chord.Count() == 0) return false;

            if (!ValidTonicBases.Contains(Tonic[0].ToString())) return false;

            if (Tonic.Count() == 2) if (Tonic[1] != 'b' && Tonic[1] != '#') return false;

            //if (Mode != null && Mode != "m") return false;

            //if (!ValidQualities.Contains(Quality)) return false;

            //if (Suspension != null && Suspension != "sus4" && Suspension != "sus2") return false;


            List<int> extList;
            if (!IsValidExtention(Extention, out extList)) return false;

            return true;
        }

        public bool IsValidExtention(string extention, out List<int> extensionList)
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
                                mod = "";
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

            if (ext != "" || mod!="")
            {
                return false;
            }

            return true;

        }

        public void DefineChord()
        {
            if (Chord.Count() > 0)
            {
                CalculateTonic(Chord);
                CalculateMode(Chord);

                if (Chord.Contains("sus"))
                {
                    CalculateSuspension(Chord);
                }
                else
                {
                    CalculateQuality(Chord);
                }

                CalculateExtentions(Chord);
            }
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
                    Mode = Modes.Major;
                }
                else
                {
                    if (chord.Count() > (_cursorPos + 1))
                    {
                        string nextChar = chord[_cursorPos + 1].ToString();
                        if (nextChar != "a")
                        {
                            Mode = Modes.Minor;
                            _cursorPos++;
                        }
                    }
                    else
                    {
                        Mode = Modes.Minor;
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
                    Suspension = SuspentionModes.sus4;
                    if (chord.Count() >= _cursorPos + 4)
                    {
                        string next = chord.Substring(_cursorPos + 3, 1);
                        if (next == "2" || next == "4")
                        {
                            if(next=="2") Suspension = SuspentionModes.sus2;
                            _cursorPos++;
                        }
                    }
                    _cursorPos += 3;
                }
            }
        }

        private void CalculateQuality(string chord)
        {
            if (chord.Count() >= _cursorPos + 3)
            {
                string q = chord.Substring(_cursorPos, 3).ToLower();
                if (q == "maj") {Quality = Qualities.maj; _cursorPos += 3; }
                if (q == "aug") {Quality = Qualities.aug; _cursorPos += 3; }
                if (q == "dim") { Quality = Qualities.dim; _cursorPos += 3; }
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

        public override string ToString()
        {
            string s = Tonic;
            if (Mode == Modes.Minor) s += "m";
            if (Quality != Qualities.none) s += Quality.ToString();
            s += Extention;
            return s;
        }

    }
}
