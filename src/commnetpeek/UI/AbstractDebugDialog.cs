using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

/* References
 * http://forum.kerbalspaceprogram.com/index.php?/topic/149324-popupdialog-and-the-dialoggui-classes/
 * 
 * 
 */

namespace commnetpeek.UI
{
    public abstract class AbstractDebugDialog
    {
        protected bool isActive;
        protected string dialogTitle;
        protected int windowWidth;
        protected int windowHeight;
        protected float positionX;
        protected float positionY;

        protected PopupDialog popupDialog;
        protected Vessel targetVessel;

        public AbstractDebugDialog(string dialogTitle, float positionX, float positionY, int windowWidth, int windowHeight)
        {
            this.isActive = false;
            this.popupDialog = null;
            this.targetVessel = null;

            this.dialogTitle = dialogTitle;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.positionX = positionX;
            this.positionY = positionY;
        }

        protected abstract bool runIntenseInfo();
        protected abstract List<DialogGUIBase> drawGUIComponents();

        public void launch(Vessel thisVessel)
        {
            if (this.isActive)
                return;

            this.isActive = true;
            this.targetVessel = thisVessel;
            if(runIntenseInfo())
                popupDialog = spawnDialog();
        }

        public void dismiss()
        {
            if (this.isActive && popupDialog != null)
            {
                popupDialog.Dismiss();
                this.isActive = false;
            }
        }

        private PopupDialog spawnDialog()
        {
            List<DialogGUIBase> entireComponentList = new List<DialogGUIBase>();

            //content
            List<DialogGUIBase> contentComponentList = drawGUIComponents();
            for(int i=0;i<contentComponentList.Count;i++)
                entireComponentList.Add(contentComponentList.ElementAt(i));

            //close button and some info
            //entireComponentList.Add(new DialogGUISpace(4));
            entireComponentList.Add(new DialogGUIHorizontalLayout(
                                        new DialogGUIBase[]
                                        {
                                            new DialogGUIFlexibleSpace(),
                                            new DialogGUIButton("Close", dismiss),
                                            new DialogGUIFlexibleSpace(),
                                            new DialogGUILabel("vX.X", false, false)
                                        }
                                    ));

            //Spawn the dialog
            MultiOptionDialog moDialog = new MultiOptionDialog("",
                                                               dialogTitle,
                                                               HighLogic.UISkin,
                                                               new Rect(0.5f, 0.5f, windowWidth, windowHeight),
                                                               entireComponentList.ToArray());

            return PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                                                new Vector2(0.5f, 0.5f),
                                                moDialog,
                                                false,
                                                HighLogic.UISkin);
        }
    }
}
