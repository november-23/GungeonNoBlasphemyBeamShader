using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GungeonNoBlasphemyBeamShader
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "spapi.etg.noblasphemybeamshader";
        public const string NAME = "No Blasphemy Beam Shader";
        public const string VERSION = "1.0.1";

        public void Awake()
        {
            var gunIds = new int[]
            {
                417, // blasphemy
                813 // alt skin
            };

            foreach (var id in gunIds)
            {
                var gun = PickupObjectDatabase.GetById(id) as Gun;
                var projectile = gun.DefaultModule.projectiles[0];
                var sprite = projectile.transform.Find("Sprite").GetComponent<tk2dBaseSprite>();

                var coll = sprite.Collection;
                var mat = coll.materials[0];

                var defs = new HashSet<tk2dSpriteDefinition>() { sprite.CurrentSprite };
                var anim = sprite.GetComponent<tk2dSpriteAnimator>();

                if (anim && anim.DefaultClip != null)
                {
                    foreach (var f in anim.DefaultClip.frames)
                    {
                        if (f == null || f.spriteCollection == null || f.spriteId < 0)
                            continue;

                        defs.Add(f.spriteCollection.spriteDefinitions[f.spriteId]);
                    }
                }

                foreach (var def in defs)
                {
                    if (def.material != null)
                        def.material = new Material(mat)
                        {
                            mainTexture = def.material.mainTexture
                        };

                    if (def.materialInst != null)
                        def.materialInst = new Material(mat)
                        {
                            mainTexture = def.materialInst.mainTexture
                        };
                }
            }
        }
    }
}
