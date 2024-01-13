using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Taglist
{
    static bool isLoadedStartScene = false;
}
enum eTag
{
    Player,
    Enemy,
    Boss,
    Item,
}
enum eEnemyType
{
    EnemyA,
    EnemyB,
    EnemyC,
    Boss,
}
enum ItemType
{
    HpRecover,
    PowerUp,
    SpawnMinion,
    Coin,
}
