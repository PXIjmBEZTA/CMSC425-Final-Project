using System.Collections;
using UnityEngine;
public interface IEnemy
{
    int HP { get; set; }
    IEnumerator Behavior1();
    //When we create behaviors 2 and 3, we will uncomment the following:
    //IEnumerator Behavior2();
    //IEnumerator Behavior3();

}
