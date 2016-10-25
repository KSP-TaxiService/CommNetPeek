using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace commnetpeek
{
    /* Reference: http://forum.kerbalspaceprogram.com/index.php?/topic/149324-popupdialog-and-the-dialoggui-classes/
     * 
     */
    public class DebugWindow
    {
        private bool isActive;

        //GUI components
        private PopupDialog transferDialog;

        int windowId = 1234789; // some random unique number

        public DebugWindow()
        {
            this.isActive = false;
        }

        public void launch()
        {
            this.isActive = true;

            transferDialog = spawnDialog();
        }

        private PopupDialog spawnDialog()
        {
            List<DialogGUIBase> dialog = new List<DialogGUIBase>();

            dialog.Add(new DialogGUIHorizontalLayout(true, false, 0, new RectOffset(), TextAnchor.UpperCenter, new DialogGUIBase[]
                {
                    new DialogGUILabel(string.Format("Transmit data to the selected vessel:\n{0}", "Text One"), false, false)
                }));

            dialog.Add(new DialogGUIHorizontalLayout(true, false, 0, new RectOffset(), TextAnchor.UpperCenter, new DialogGUIBase[]
            {
            new DialogGUIToggle(false, "Transfer All Open Data",
                delegate(bool b)
                {
                    CNPLog.Debug("radio button pressed");
                }, 200, 20)
            }));
            

            List<DialogGUIHorizontalLayout> vessels = new List<DialogGUIHorizontalLayout>();

            for (int i = 1; i >= 0; i--)
            {
                DialogGUILabel label = null;

                if (true)
                {
                    string transmit = string.Format("Xmit: {0:P0}", 66.99);

                    if (true)
                        transmit += string.Format("(+{0:P0})", 1337);

                    label = new DialogGUILabel(transmit, 130, 25);
                }

                DialogGUIBase button = null;

                if (true)
                {
                    button = new DialogGUIButton(
                                            "some vessel name",
                                            delegate
                                            {
                                                CNPLog.Debug("Button pressed");
                                            },
                                            160,
                                            30,
                                            true,
                                            null);

                    button.size = new Vector2(160, 30);
                }

                DialogGUIHorizontalLayout h = new DialogGUIHorizontalLayout(true, false, 4, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUIBase[] { button });

                if (label != null)
                    h.AddChild(label);

                vessels.Add(h);
            }

            DialogGUIBase[] scrollList = new DialogGUIBase[vessels.Count + 1];

            scrollList[0] = new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, true);

            for (int i = 0; i < vessels.Count; i++)
                scrollList[i + 1] = vessels[i];

            dialog.Add(new DialogGUIScrollList(Vector2.one, false, true,
                new DialogGUIVerticalLayout(10, 100, 4, new RectOffset(6, 24, 10, 10), TextAnchor.MiddleLeft, scrollList)
                ));

            dialog.Add(new DialogGUISpace(4));

            dialog.Add(new DialogGUIHorizontalLayout(new DialogGUIBase[]
            {
                new DialogGUIFlexibleSpace(),
                new DialogGUIButton("Cancel Transfer", popupDismiss),
                new DialogGUIFlexibleSpace(),
                new DialogGUILabel("6.9", false, false)
            }));

            RectTransform resultRect = null; // resultsDialog.GetComponent<RectTransform>();

            Rect pos = new Rect(0.5f, 0.5f, 300, 300);

            if (resultRect != null)
            {
                Vector2 resultPos = resultRect.position;

                float scale = GameSettings.UI_SCALE;

                int width = Screen.width;
                int height = Screen.height;

                float xpos = (resultPos.x / scale) + width / 2;
                float ypos = (resultPos.y / scale) + height / 2;

                float yNorm = ypos / height;

                pos.y = yNorm;

                pos.x = xpos > (width - (550 * scale)) ? (xpos - 360) / width : (xpos + 360) / width;
            }

            return PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("", "Science Relay", UISkinManager.defaultSkin, pos, dialog.ToArray()), false, UISkinManager.defaultSkin);
        }

        private void popupDismiss()
        {
            if (transferDialog != null)
                transferDialog.Dismiss();
        }
    }
}
