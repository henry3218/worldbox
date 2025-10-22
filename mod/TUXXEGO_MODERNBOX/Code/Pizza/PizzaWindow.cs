using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NCMS.Utils;
using System.Text.RegularExpressions;
using System.Reflection;
namespace ModernBox
{
    class PizzaWindow
    {
		public static int MoveDown = -50;
        private static ScrollWindow window;
        private static GameObject content;

    public static void init()
    {
        window = Windows.CreateNewWindow("PizzaWindow", "ModernBox");

        var scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View");
        scrollView.gameObject.SetActive(true);

        var viewport = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View/Viewport");
        var viewportRect = viewport.GetComponent<RectTransform>();
        viewportRect.sizeDelta = new Vector2(0, 17);

        content = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View/Viewport/Content");

			          string gold = "#FFD700";
					  string Dgold = "#ffae00";
					  var description =
			@"<color='" + gold + @"'>Welcome to Tux's Pizza shop! I have to work on the multiplayer mod so you will take over my duty as a pizza shop owner for a bit. Please treat my employees (myoppie, dank, and morfos) with respect! oh, and to leave pizza mode, just restart the game.</color>
			";
					  var name = window.transform.Find("Background").Find("Name").gameObject;
					  var nameText = name.GetComponent<Text>();
					  nameText.text = description;
					  nameText.color = new Color(0.9f, 0.6f, 0, 1);
					  nameText.fontSize = 10;
					  nameText.alignment = TextAnchor.UpperCenter;
					  nameText.supportRichText = true;
					  name.transform.SetParent(window.transform.Find("Background").Find("Scroll View").Find("Viewport").Find("Content"));
					  name.SetActive(true);
					  var nameRect = name.GetComponent<RectTransform>();
					  nameRect.anchorMin = new Vector2(0.5f, 1);
					  nameRect.anchorMax = new Vector2(0.5f, 1);
					  nameRect.offsetMin = new Vector2(-90f, nameText.preferredHeight * -1);
					  nameRect.offsetMax = new Vector2(90f, -17);
					  nameRect.sizeDelta = new Vector2(180, nameText.preferredHeight + 50);
					  window.GetComponent<RectTransform>().sizeDelta = new Vector2(0, nameText.preferredHeight + 50);
					  name.transform.localPosition = new Vector2(name.transform.localPosition.x, ((nameText.preferredHeight / 2) + 30) * -1);

    }
  }
}