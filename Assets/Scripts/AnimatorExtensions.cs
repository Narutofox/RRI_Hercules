using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static partial class AnimatorExtensions
    {
        public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
        {
            var parameters = self.parameters;
            foreach (var currParam in parameters)
            {
                if (currParam.type == type && currParam.name == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
