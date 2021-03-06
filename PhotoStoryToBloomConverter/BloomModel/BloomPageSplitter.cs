﻿using PhotoStoryToBloomConverter.BloomModel.BloomHtmlModel;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;

namespace PhotoStoryToBloomConverter.BloomModel
{
    public class BloomPageSplitter
    {
        public string Text;
        public BloomImage Image;
        public BloomAudio Audio;
        

        public Div ConvertToHtml()
        {
            var imageDiv = Image.ConvertToHtml();
	        var contentText = string.IsNullOrWhiteSpace(Text) ? "nbsp;" : Text;
            var narrationSpan = (Audio.NarrationPath == null) ? null :
				new Span { Class = "audio-sentence", Id = Path.GetFileNameWithoutExtension(Audio.NarrationPath),
					ContentText = contentText, Duration = Audio.Duration};
            return new Div
            {
                Class = "split-pane horizontal-percent",
                Style = "min-height: 42px;",
                Divs = new List<Div>
                {
                    new Div
                    {
                        Class = "split-pane-component position-top",
                        Style = "bottom: 50%;",
                        Divs = new List<Div>
                        {
                            new Div
                            {
                                Class = "split-pane-component-inner",
                                Style = "position: relative;",
                                MinHeight = "60px 150px 250px",
                                MinWidth = "60px 150px 250px",
                                Divs = new List<Div>
                                {
                                    imageDiv,
                                }
                            }
                        }
                    },
                    new Div
                    {
                        Class = "split-pane-divider horizontal-divider",
                        Style = "bottom: 50%;",
                        Title = "50%",
                        SimpleText = ""
                    },
                    new Div
                    {
                        Class = "split-pane-component position-bottom",
                        Style = "height: 50%;",
                        Divs = new List<Div>
                        {
                            new Div 
                            {
                                Class = "split-pane-component-inner adding",
                                Style = "position: relative;",
                                MinWidth = "60px 150px 250px",
                                Divs = new List<Div>
                                {
                                    new Div
                                    {
                                        Class = "box-header-off bloom-translationGroup",
                                        Divs = new List<Div>
                                        {
                                            new Div
                                            {
                                                Class = "bloom-editable cke_editablecke_editable_inline cke_contents_ltr bloom-content1",
                                                ContentEditable = "true",
                                                Lang = "en",
                                                TabIndex = "0",
                                                Spellcheck = "true",
                                                Role = "textbox",
                                                AriaLabel = "false",
                                                FormattedText = new Paragraph
                                                {
                                                    Text = ""
                                                }
                                            }
                                        }
                                    },
                                    new Div
                                    {
                                        Class = "bloom-translationGroup bloom-trailingElement normal-style",
                                        Divs = new List<Div>
                                        {
                                            new Div
                                            {
                                                Class = "bloom-editable cke_editable cke_editable_inline cke_contents_ltr normal-style bloom-content1",
                                                ContentEditable = "true",
                                                Lang = "en",
                                                Style = "min-height: 24px;",
                                                TabIndex = "0",
                                                Spellcheck = "true",
                                                Role = "textbox",
                                                AriaLabel = "false",
                                                DataLanguageTipContent = "English",
                                                FormattedText = new Paragraph
                                                {
                                                    Span = narrationSpan
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
	}
}
