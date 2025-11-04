using System.Collections;
using UnityEngine;
public interface IEnemy
{
    int HP { get; set; }
    IEnumerator Behavior1();
    IEnumerator Behavior2();
    IEnumerator Behavior3();

}
