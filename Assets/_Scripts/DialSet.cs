﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using VRTK;
using System.Linq;
using System;

public class DialSet : HasWatchableBool {
	public Dial[] dials;
	public string combination;
    public string startingValue;
	public UnityEvent onUnlock;
	private bool everUnlocked = false;
    private VRTK_InteractableObject[] dialIOs;


	// Use this for initialization
	void Start ()
    {
        if (startingValue != string.Empty)
        {
            for (int i = 0; i < dials.Length; i++)
            {
                dials[i].SetValue(startingValue[i]);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (dialIOs == null){
            InitDialIOs();
        }

        var currentlyUsing = dialIOs.Where(io => io.IsUsing()).FirstOrDefault();
        if (currentlyUsing != null)
        {
            DisableAllIOsBut(currentlyUsing);
        }
        else
        {
            EnableAllIOs();
        }

        if (!everUnlocked && GetValue () == combination) {
			Unlock ();
		}

        boolValue = GetValue() == combination;
	}

    private void EnableAllIOs()
    {    
        foreach (var dialIO in dialIOs)
        {
            dialIO.enabled = true;
        }
    }

    private void DisableAllIOsBut(VRTK_InteractableObject currentlyUsing)
    {
        foreach (var dialIO in dialIOs)
        {
            if (dialIO != currentlyUsing)
            {
                dialIO.enabled = false;
            }
        }
    }

    void InitDialIOs()
    {
        dialIOs = new VRTK_InteractableObject[dials.Length];
        for (int i = 0; i < dials.Length; i++)
        {
            dialIOs[i] = dials[i].GetComponent<VRTK_InteractableObject>();
        }
    }

	void Unlock() {
		everUnlocked = true;
		onUnlock.Invoke ();
	}

	public string GetValue() {
		string value = "";
		for (int i = 0; i < dials.Length; i++) {
			value += dials [i].GetValue();
		}
		return value;
	}

    public string GetIncrementalValue()
    {
        string value = "";
        for (int i = 0; i < dials.Length; i++)
        {
            value += dials[i].GetIncrementalValue();
        }
        return value;
    }

    public void MagicUnlock()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].SetValue(combination[i]);
        }
    }
}
