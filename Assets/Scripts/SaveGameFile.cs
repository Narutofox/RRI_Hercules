using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class SaveGameFile
    {
        public Transform Position { get; set; }
        public string Level { get; set; }
        public PlayerHealth PlayerHealth { get; set; }
        public PlayerAttack PlayerAttack { get; set; }
    }
}
