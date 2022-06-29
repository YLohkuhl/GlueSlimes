using SRML.SR;
using SRML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GlueSlime
{
    class GlueSlimePlort
    {
        public static GameObject GluePlort()
        {
            GameObject Prefab = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_PLORT)); //It can be any plort, but pink works the best. 
            Prefab.name = "Glue Plort";

            Prefab.GetComponent<Identifiable>().id = ModdedIds.ModdedIds.GLUE_PLORT;
            Prefab.GetComponent<Vacuumable>().size = Vacuumable.Size.NORMAL;

            Prefab.GetComponent<MeshRenderer>().material = Object.Instantiate<Material>(Prefab.GetComponent<MeshRenderer>().material);
            Color PureWhite = new Color32(255, 255, 255, byte.MaxValue); // RGB   
            Color White = Color.white;
            //Pretty self explanatory. These change the color of the plort. You can set the colors to whatever you want.    
            Prefab.GetComponent<MeshRenderer>().material.SetColor("_TopColor", White);
            Prefab.GetComponent<MeshRenderer>().material.SetColor("_MiddleColor", PureWhite);
            Prefab.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", White);

            LookupRegistry.RegisterIdentifiablePrefab(Prefab);

            return Prefab;
        }
    }
}
