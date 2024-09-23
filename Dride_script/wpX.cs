using UnityEngine;
using UnityEngine.AI;

// NavMeshAgentコンポーネントがアタッチされていない場合アタッチ
[RequireComponent(typeof(NavMeshAgent))]
public class wpX : MonoBehaviour
{
    [SerializeField]
    [Tooltip("巡回する地点の配列")]
    private Transform[] waypoints;

    // NavMeshAgentコンポーネントを入れる変数
    private NavMeshAgent navMeshAgent;
    // 現在の目的地
    [SerializeField]private int currentWaypointIndex;
    int r=0;

    // Start is called before the first frame update
    void Start()
    {
        rs();
        // navMeshAgent変数にNavMeshAgentコンポーネントを入れる
        navMeshAgent = GetComponent<NavMeshAgent>();
        // 最初の目的地を入れる
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        // 目的地点までの距離(remainingDistance)が目的地の手前までの距離(stoppingDistance)以下になったら
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if(currentWaypointIndex==11 || currentWaypointIndex==24 || currentWaypointIndex==35 || currentWaypointIndex==41 || currentWaypointIndex==49){
                rs();
            }else{
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                if(currentWaypointIndex==29){
                    r=Random.Range(0,2);
                    if(r==1){
                        currentWaypointIndex=36;
                    }
                }
            }
            // 目的地を次の場所に設定
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
    void rs(){
        r=Random.Range(0,4);
        switch(r){
            case 0:currentWaypointIndex=0; break;
            case 1:currentWaypointIndex=12; break;
            case 2:currentWaypointIndex=25; break;
            case 3:currentWaypointIndex=42; break;
        }
    }
}