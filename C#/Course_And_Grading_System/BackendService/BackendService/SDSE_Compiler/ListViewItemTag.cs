using System;

namespace Compiler
{
    struct ListViewItemTag2
    {
        public int ImagesIndex;
        public string ToolTipText;
        public string ListText;
        public string ImportText;

        public ListViewItemTag2(int imgIndex, string toolTip, string listText, string importText)
        {
            ImagesIndex = imgIndex;
            ToolTipText = toolTip;
            ListText = listText;
            ImportText = importText;
        }
    }
}
