using UnityEngine;
using System.Reflection;

public class Util
{
    public static T createPrefabs<T>(string nameInHierachy) where T : Component
    {
        GameObject go = new GameObject(nameInHierachy);
        go.hideFlags = HideFlags.HideInHierarchy;
        go.SetActive(false);

        go.transform.localScale = new Vector3(1, 1, 1);
        
#if false
        var type = System.Type.GetType(nameInHierachy);
        if (type != null)
            go.AddComponent(type);
#endif
        return go.AddComponent<T>();
   }

#if flase

    public static T GetReference<T>(object inObj, string fieldName) where T : class
    {
        return GetField(inObj, fieldName) as T;
    }


    public static T GetValue<T>(object inObj, string fieldName) where T : struct
    {
        return (T)GetField(inObj, fieldName);
    }

    public static void SetField(object inObj, string fieldName, object newValue)
    {
        FieldInfo info = inObj.GetType().GetField(fieldName);
        if (info != null)
            info.SetValue(inObj, newValue);
    }


    private static object GetField(object inObj, string fieldName)
    {
        object ret = null;
        FieldInfo info = inObj.GetType().GetField(fieldName);
        if (info != null)
            ret = info.GetValue(inObj);
        return ret;
    }
#endif
}
