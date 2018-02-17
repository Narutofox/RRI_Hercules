using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class SaveGameFile
    {
        public float x;
        public float y;
        public string Level;
        public int PlayerMaxHealth;
        public int PlayerHealth;
        public int NumberOfFireballShots;
        public float NumberOfLightningSeconds;
        public float NumberOfInvincibleSeconds;
        public int Points;
        public int TotalPoints;
        public string PlayerName;
    }
}
