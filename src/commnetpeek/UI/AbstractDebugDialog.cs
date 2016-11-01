﻿using System;
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
        protected bool isDisplayed;
        protected string dialogTitle;
        protected int windowWidth;
        protected int windowHeight;
        protected float normalizedCenterX; //0.0f to 1.0f
        protected float normalizedCenterY; //0.0f to 1.0f

        protected PopupDialog popupDialog;

        public AbstractDebugDialog(string dialogTitle, float normalizedCenterX, float normalizedCenterY, int windowWidth, int windowHeight)
        {
            this.isDisplayed = false;
            this.popupDialog = null;
            //this.targetVessel = null;

            this.dialogTitle = dialogTitle;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.normalizedCenterX = normalizedCenterX;
            this.normalizedCenterY = normalizedCenterY;
        }

        protected abstract bool runIntenseInfo(Vessel thisVessel, Part commandPart);
        protected abstract List<DialogGUIBase> drawContentComponents();

        public void launch(Vessel thisVessel, Part commandPart)
        {
            if (this.isDisplayed)
                return;

            this.isDisplayed = true;
            if(runIntenseInfo(thisVessel, commandPart))
                popupDialog = spawnDialog();
        }

        public void dismiss()
        {
            if (this.isDisplayed && popupDialog != null)
            {
                popupDialog.Dismiss();
                this.isDisplayed = false;
            }
        }

        private PopupDialog spawnDialog()
        {
            /* This dialog looks like below
             * -----------------------
             * |        TITLE        |
             * |----------------------
             * |                     |
             * |      CONTENT        |
             * |                     |
             * |----------------------
             * |       CLOSE      XX |
             * ----------------------- 
             */

            List<DialogGUIBase> entireComponentList = new List<DialogGUIBase>();

            //content
            List<DialogGUIBase> contentComponentList = drawContentComponents();
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
                                                               new Rect(normalizedCenterX, normalizedCenterY, windowWidth, windowHeight),
                                                               entireComponentList.ToArray());

            return PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                                                new Vector2(0.5f, 0.5f),
                                                moDialog,
                                                true,
                                                HighLogic.UISkin);
        }
    }
}
