﻿using CardsEditor.Model;
using LevelSetsEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class TagVM : INPCBase
    {
        #region Constructors
        public TagVM(Tag tag)
        {
            _tag = tag;
        }
        #endregion

        #region Fields
        Tag _tag;
        #endregion

        #region Properties
        public Tag Tag
        {
            get { return _tag; }
            set { _tag = value; OnPropertyChanged("Tag"); }
        }

        public int Id { get { return Tag.Id; } }

        public string Name
        {
            get { return Tag.Name; }
            set
            {
                Tag.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string SoundedText { get { return Tag.SoundedText; } set { Tag.SoundedText = value; OnPropertyChanged("SoundedText"); } }

        public string TagGroup
        {
            get { return Tag.TagGroup; }
            set
            {
                Tag.TagGroup = value;
                OnPropertyChanged("TagGroup");
            }
        }

        public string FirstCardTitle
        {
            get
            {
                if (Tag.Cards == null)
                    return "";
                else
                   return Tag.Cards.FirstOrDefault().Title;
            }
        }

        #endregion

        #region Methods
        #endregion

        #region Commands

        #endregion

    }
}
