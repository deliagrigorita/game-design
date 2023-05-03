using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using CodeMonkey.Utils;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    // private void Start() {
    //     FunctionPeriodic.Create(() =>{
    //         float startTime = Time.realtimeSinceStartup;

    //         int findPathJobCount = 20;
    //         NativeArray<JobHandle> jobHandleArray = new NativeArray<JobHandle>(findPathJobCount, Allocator.TempJob);

    //         for(int i = 0; i < findPathJobCount; i++){
    //             FindPathJob findPathJob = new FindPathJob {
    //                 startingPosition = new int2(0, 0), 
    //                 endPosition = new int2(99, 99)
    //             };
    //             jobHandleArray[i] = findPathJob.Schedule();
    //         }

    //         JobHandle.CompleteAll(jobHandleArray);
    //         jobHandleArray.Dispose();

    //         Debug.Log("Time: " + ((Time.realtimeSinceStartup - startTime) * 1000f));
    //     }, 1f);
    // }
 
    [BurstCompile]
    private struct FindPathJob : IJob{

        public int2 startingPosition;
        public int2 endPosition;

        public void Execute(){
            int2 gridSize = new int2(100, 100);

            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

            for (int x = 0; x < gridSize.x; x++){
                for(int y = 0; y < gridSize.y; y++){
                    PathNode pathNode = new PathNode();
                    pathNode.x = x;
                    pathNode.y = y;
                    pathNode.index = CalculateIndex(x, y, gridSize.x);

                    pathNode.gCost = int.MaxValue;
                    pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                    pathNode.CalculateFCost();

                    pathNode.isWalkable = true;
                    pathNode.cameFromIndex = -1;

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            // PathNode walkablePathNode = pathNodeArray[CalculateIndex(1, 0, gridSize.x)];
            // walkablePathNode.SetIsWalkable(false);
            // pathNodeArray[CalculateIndex(1, 0, gridSize.x)] = walkablePathNode;

            // walkablePathNode = pathNodeArray[CalculateIndex(1, 1, gridSize.x)];
            // walkablePathNode.SetIsWalkable(false);
            // pathNodeArray[CalculateIndex(1, 1, gridSize.x)] = walkablePathNode;

            // walkablePathNode = pathNodeArray[CalculateIndex(1, 2, gridSize.x)];
            // walkablePathNode.SetIsWalkable(false);
            // pathNodeArray[CalculateIndex(1, 2, gridSize.x)] = walkablePathNode;



            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
            neighbourOffsetArray[0] = new int2(-1, 0); //Left
            neighbourOffsetArray[1] = new int2(+1, 0); //Right
            neighbourOffsetArray[2] = new int2(0, +1); //Up
            neighbourOffsetArray[3] = new int2(0, -1); //Down
            neighbourOffsetArray[4] = new int2(-1, -1); //Down left
            neighbourOffsetArray[5] = new int2(-1, +1); //Up left
            neighbourOffsetArray[6] = new int2(+1, -1); //Down right
            neighbourOffsetArray[7] = new int2(+1, +1); //Up right


            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

            PathNode startNode = pathNodeArray[CalculateIndex(startingPosition.x, startingPosition.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            while(openList.Length > 0){
                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if(currentNodeIndex == endNodeIndex){
                    // reached end
                    break;
                }

                // Remove current node from list
                for(int i = 0; i < openList.Length; i++){
                    if(openList[i] == currentNodeIndex){
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }
                
                closedList.Add(currentNodeIndex);

                for(int i = 0; i < neighbourOffsetArray.Length; i++){
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if(!IsPositionInsideGrid(neighbourPosition, gridSize)){
                        //Pos outside grid
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);
                    if(closedList.Contains(neighbourNodeIndex)){
                        //Already serached this pos
                        continue;
                    }

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if(!neighbourNode.isWalkable){
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                    if(tentativeGCost < neighbourNode.gCost){
                        neighbourNode.cameFromIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.CalculateFCost();
                        pathNodeArray[neighbourNodeIndex] = neighbourNode;

                        if(!openList.Contains(neighbourNode.index)){
                            openList.Add(neighbourNode.index);
                        }
                    }
                }
            }

            PathNode endNode = pathNodeArray[endNodeIndex];
            if(endNode.cameFromIndex == -1){
                //Didnt find a path!
            }else{
                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
                // foreach(int2 pathPosition in path)
                // {
                //     Debug.Log(pathPosition);
                // }
                path.Dispose();
            }
            
            pathNodeArray.Dispose();
            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
      
        }

        private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode){
            if(endNode.cameFromIndex == -1){
                return new NativeList<int2>(Allocator.Temp);
            }else{
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.x, endNode.y));

                PathNode currentNode = endNode;
                while(currentNode.cameFromIndex != -1){
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }
                return path;
            }
        }

        private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize){
            return gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < gridSize.x && gridPosition.y < gridSize.y;
        }

        private int CalculateIndex(int x, int y, int gridWidth){
            return x + y * gridWidth;

        }

        private int CalculateDistanceCost(int2 sPosition, int2 ePosition){
            int xDistance = math.abs(sPosition.x - ePosition.x);
            int yDistance = math.abs(sPosition.y - ePosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray){
            PathNode lowestCostPathNode = pathNodeArray[openList[0]];
            for (int i = 1; i < openList.Length; i++){
                PathNode testPathNode = pathNodeArray[openList[i]];
                if ( testPathNode.fCost < lowestCostPathNode.fCost){
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }

        private struct PathNode{
            public int x;
            public int y;

            public int index;

            public int gCost;
            public int hCost;
            public int fCost;

            public bool isWalkable;

            public int cameFromIndex;

            public void CalculateFCost(){
                fCost = gCost + hCost;
            }

            public void SetIsWalkable(bool isWalkable){
                this.isWalkable = isWalkable;
            }
        }
    }

    private void FindPath(int2 startingPosition, int2 endPosition){
        int2 gridSize = new int2(20, 20);

        NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

        for (int x = 0; x < gridSize.x; x++){
            for(int y = 0; y < gridSize.y; y++){
                PathNode pathNode = new PathNode();
                pathNode.x = x;
                pathNode.y = y;
                pathNode.index = CalculateIndex(x, y, gridSize.x);

                pathNode.gCost = int.MaxValue;
                pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                pathNode.CalculateFCost();

                pathNode.isWalkable = true;
                pathNode.cameFromIndex = -1;

                pathNodeArray[pathNode.index] = pathNode;
            }
        }

        // PathNode walkablePathNode = pathNodeArray[CalculateIndex(1, 0, gridSize.x)];
        // walkablePathNode.SetIsWalkable(false);
        // pathNodeArray[CalculateIndex(1, 0, gridSize.x)] = walkablePathNode;

        // walkablePathNode = pathNodeArray[CalculateIndex(1, 1, gridSize.x)];
        // walkablePathNode.SetIsWalkable(false);
        // pathNodeArray[CalculateIndex(1, 1, gridSize.x)] = walkablePathNode;

        // walkablePathNode = pathNodeArray[CalculateIndex(1, 2, gridSize.x)];
        // walkablePathNode.SetIsWalkable(false);
        // pathNodeArray[CalculateIndex(1, 2, gridSize.x)] = walkablePathNode;



        NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
        neighbourOffsetArray[0] = new int2(-1, 0); //Left
        neighbourOffsetArray[1] = new int2(+1, 0); //Right
        neighbourOffsetArray[2] = new int2(0, +1); //Up
        neighbourOffsetArray[3] = new int2(0, -1); //Down
        neighbourOffsetArray[4] = new int2(-1, -1); //Down left
        neighbourOffsetArray[5] = new int2(-1, +1); //Up left
        neighbourOffsetArray[6] = new int2(+1, -1); //Down right
        neighbourOffsetArray[7] = new int2(+1, +1); //Up right

        int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

        PathNode startNode = pathNodeArray[CalculateIndex(startingPosition.x, startingPosition.y, gridSize.x)];
        startNode.gCost = 0;
        startNode.CalculateFCost();
        pathNodeArray[startNode.index] = startNode;

        NativeList<int> openList = new NativeList<int>(Allocator.Temp);
        NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

        openList.Add(startNode.index);

        while(openList.Length > 0){
            int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
            PathNode currentNode = pathNodeArray[currentNodeIndex];

            if(currentNodeIndex == endNodeIndex){
                // reached end
                break;
            }

            // Remove current node from list
            for(int i = 0; i < openList.Length; i++){
                if(openList[i] == currentNodeIndex){
                    openList.RemoveAtSwapBack(i);
                    break;
                }
            }
            
            closedList.Add(currentNodeIndex);

            for(int i = 0; i < neighbourOffsetArray.Length; i++){
                int2 neighbourOffset = neighbourOffsetArray[i];
                int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                if(!IsPositionInsideGrid(neighbourPosition, gridSize)){
                    //Pos outside grid
                    continue;
                }

                int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);
                if(closedList.Contains(neighbourNodeIndex)){
                    //Already serached this pos
                    continue;
                }

                PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                if(!neighbourNode.isWalkable){
                    continue;
                }

                int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                if(tentativeGCost < neighbourNode.gCost){
                    neighbourNode.cameFromIndex = currentNodeIndex;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.CalculateFCost();
                    pathNodeArray[neighbourNodeIndex] = neighbourNode;

                    if(!openList.Contains(neighbourNode.index)){
                        openList.Add(neighbourNode.index);
                    }
                }
            }
        }

        PathNode endNode = pathNodeArray[endNodeIndex];
        if(endNode.cameFromIndex == -1){
            //Didnt find a path!
        }else{
            NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
            // foreach(int2 pathPosition in path)
            // {
            //     Debug.Log(pathPosition);
            // }
            path.Dispose();
        }
         
        pathNodeArray.Dispose();
        neighbourOffsetArray.Dispose();
        openList.Dispose();
        closedList.Dispose();
    }

    private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode){
        if(endNode.cameFromIndex == -1){
            return new NativeList<int2>(Allocator.Temp);
        }else{
            NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
            path.Add(new int2(endNode.x, endNode.y));

            PathNode currentNode = endNode;
            while(currentNode.cameFromIndex != -1){
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromIndex];
                path.Add(new int2(cameFromNode.x, cameFromNode.y));
                currentNode = cameFromNode;
            }
            return path;
        }
    }

    private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize){
        return gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < gridSize.x && gridPosition.y < gridSize.y;
    }

    private int CalculateIndex(int x, int y, int gridWidth){
        return x + y * gridWidth;

    }

    private int CalculateDistanceCost(int2 sPosition, int2 ePosition){
        int xDistance = math.abs(sPosition.x - ePosition.x);
        int yDistance = math.abs(sPosition.y - ePosition.y);
        int remaining = math.abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray){
        PathNode lowestCostPathNode = pathNodeArray[openList[0]];
        for (int i = 1; i < openList.Length; i++){
            PathNode testPathNode = pathNodeArray[openList[i]];
            if ( testPathNode.fCost < lowestCostPathNode.fCost){
                lowestCostPathNode = testPathNode;
            }
        }
        return lowestCostPathNode.index;
    }

    private struct PathNode{
        public int x;
        public int y;

        public int index;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool isWalkable;

        public int cameFromIndex;

        public void CalculateFCost(){
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool isWalkable){
            this.isWalkable = isWalkable;
        }
    }
}
