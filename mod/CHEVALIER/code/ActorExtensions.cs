using UnityEngine;

namespace VideoCopilot.code.utils
{
    public static class ActorExtensions
    {
        private const string Chevalier_key = "wushu.ChevalierNum";
        private const string lifespan_key = "strings.S.lifespan";
        private const string Comprehension_key = "ComprehensionNum";

        public static float GetChevalier(this Actor actor)
        {
            actor.data.get(Chevalier_key, out float val, 0);
            return val;
        }

        public static void SetChevalier(this Actor actor, float val)
        {
            actor.data.set(Chevalier_key, val);
        }

        public static void ChangeChevalier(this Actor actor, float delta)
        {
            actor.data.get(Chevalier_key, out float val, 0);
            val += delta;
            actor.data.set(Chevalier_key, Mathf.Max(0, val));
        }
        
        public static float GetComprehension(this Actor actor)
        {
            actor.data.get(Comprehension_key, out float val, 0);
            return val;
        }
        
        public static void SetComprehension(this Actor actor, float val)
        {
            actor.data.set(Comprehension_key, val);
        }
        
        public static void ChangeComprehension(this Actor actor, float delta)
        {
            actor.data.get(Comprehension_key, out float val, 0);
            val += delta;
            actor.data.set(Comprehension_key, Mathf.Max(0, val));
        }
    }
}