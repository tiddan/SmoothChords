using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace PianoLibrary.Model.Chords.Voicings
{
    public class DefaultVoicing : ChordVoicing
    {
        public DefaultVoicing(ChordComposition composition)
            : base(composition)
        {
        }

        public override ObservableCollection<int> Compose()
        {
            ChordDefinition rightHand = Composition.Chords[0];
            Hashtable rightHandNotes = new Hashtable();
            Hashtable rightHandModifiedKeys = new Hashtable();

            ChordDefinition leftHand = Composition.Chords[Composition.Chords.Count() - 1];
            Hashtable leftHandNotes = new Hashtable();

            if (Composition.IsValidNotation())
            {
                int baseLineOffset = (2 * 12);
                int chordOffset = (3 * 12);
                

                PianoKey.KeyTypes tonicKey;
                if (Enum.TryParse<PianoKey.KeyTypes>(rightHand.Tonic[0].ToString(), out tonicKey))
                {
                    int tonicOffset = Piano.GetOffset(tonicKey);

                    if (rightHand.Tonic.Count() == 2)
                    {
                        if (rightHand.Tonic[1] == 'b') tonicOffset--;
                        if (rightHand.Tonic[1] == '#') tonicOffset++;
                    }

                    // Add chord-note "I"
                    rightHandNotes.Add("1", chordOffset + tonicOffset);

                    // Add chord-note "III"
                    rightHandNotes.Add("3", chordOffset + tonicOffset + Piano.OFFSET_3RD);

                    // Add chord-note "V"
                    rightHandNotes.Add("5", chordOffset + tonicOffset + Piano.OFFSET_5TH);

                    // Add Mode
                    if (rightHand.Mode == ChordDefinition.Modes.Minor)
                    {
                        if (rightHandNotes.ContainsKey("3"))
                        {
                            rightHandNotes["3"] = (int)rightHandNotes["3"] - 1;
                            rightHandModifiedKeys.Add("3", -1);
                        }
                    }

                    // Add Quality
                    if (rightHand.Quality != null)
                    {
                        if (rightHand.Quality == ChordDefinition.Qualities.maj)
                        {
                            rightHandNotes["7"] = chordOffset + tonicOffset + Piano.OFFSET_7TH + 1;
                            rightHandModifiedKeys.Add("7", 0);
                        }
                        else if (rightHand.Quality == ChordDefinition.Qualities.aug)
                        {
                            rightHandNotes["5"] = (int)rightHandNotes["5"] + 1;
                            rightHandModifiedKeys["5"] = +1;
                        }
                        else if (rightHand.Quality == ChordDefinition.Qualities.dim)
                        {
                            if (!rightHandModifiedKeys.ContainsKey("3"))
                            {
                                rightHandNotes["3"] = (int)rightHandNotes["3"] - 1;
                                rightHandModifiedKeys["3"] = -1;
                            }
                            rightHandNotes["5"] = (int)rightHandNotes["5"] - 1;
                            rightHandModifiedKeys["5"] = -1;
                        }
                    }

                    // Add Suspension
                    if (rightHand.Suspension != null)
                    {
                        if (!rightHandModifiedKeys.ContainsKey("3"))
                        {
                            if (rightHand.Suspension == ChordDefinition.SuspentionModes.sus4)
                            {
                                rightHandNotes["3"] = (int)rightHandNotes["3"] + 1;
                                rightHandModifiedKeys["3"] = +1;
                            }
                            else if(rightHand.Suspension == ChordDefinition.SuspentionModes.sus2)
                            {
                                rightHandNotes["3"] = (int)rightHandNotes["3"] - 2;
                                rightHandModifiedKeys["3"] = -2;
                            }
                        }
                    }

                    // Add Extentions
                    if (rightHand.Extention != null)
                    {
                        List<int> extList;
                        rightHand.IsValidExtention(rightHand.Extention, out extList);

                        foreach (int ext in extList)
                        {

                            int modifier = 0;
                            int index = rightHand.Extention.IndexOf(ext.ToString());
                            if (index > 0)
                            {
                                string token = rightHand.Extention.ElementAt(index - 1).ToString();
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
                                        rightHandNotes["5"] = (int)rightHandNotes["5"] + modifier;
                                        break;
                                    }
                                case 7:
                                    {
                                        if (!rightHandModifiedKeys.ContainsKey("7"))
                                        {
                                            rightHandNotes["7"] = chordOffset + tonicOffset + Piano.OFFSET_7TH + modifier;
                                            rightHandModifiedKeys["7"] = 0;
                                        }
                                        break;
                                    }
                                case 9:
                                    {
                                        if (!rightHandModifiedKeys.ContainsKey("9"))
                                        {
                                            rightHandNotes["9"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_9TH + modifier;
                                            rightHandModifiedKeys["9"] = 0;
                                        }
                                        break;
                                    }
                                case 11:
                                    {
                                        if (!rightHandModifiedKeys.ContainsKey("9"))
                                        {
                                            rightHandNotes["9"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_9TH;
                                            rightHandModifiedKeys["9"] = 0;
                                        }
                                        if (!rightHandModifiedKeys.ContainsKey("11"))
                                        {
                                            rightHandNotes["11"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_11TH + modifier;
                                            rightHandModifiedKeys["9"] = 0;
                                        }
                                        break;
                                    }
                                case 13:
                                    {
                                        if (!rightHandModifiedKeys.ContainsKey("9"))
                                        {
                                            rightHandNotes["9"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_9TH;
                                            rightHandModifiedKeys["9"] = 0;
                                        }
                                        if (!rightHandModifiedKeys.ContainsKey("11"))
                                        {
                                            rightHandNotes["11"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_11TH;
                                            rightHandModifiedKeys["11"] = 0;
                                        }
                                        if (!rightHandModifiedKeys.ContainsKey("13"))
                                        {
                                            rightHandNotes["13"] = chordOffset + 12 + tonicOffset + Piano.OFFSET_13TH + modifier;
                                            rightHandModifiedKeys["13"] = 0;
                                        }
                                        break;
                                    }

                            }
                        }
                    }
                }
                if (Enum.TryParse<PianoKey.KeyTypes>(leftHand.Tonic[0].ToString(), out tonicKey))
                {
                    int tonicOffset = Piano.GetOffset(tonicKey);

                    if (leftHand.Tonic.Count() == 2)
                    {
                        if (leftHand.Tonic[1] == 'b') tonicOffset--;
                        if (leftHand.Tonic[1] == '#') tonicOffset++;
                    }

                    // Add chord-note "I"
                    leftHandNotes.Add("1", baseLineOffset + tonicOffset);
                }
            }
            else
            {
                return new ObservableCollection<int>();
            }

            ObservableCollection<int> notes = new ObservableCollection<int>();
            foreach (int value in leftHandNotes.Values) notes.Add(value);
            foreach (int value in rightHandNotes.Values) notes.Add(value);
            return notes;
        }

    }
}
