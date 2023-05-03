using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.IO;

public class GenerateLevel : MonoBehaviour {

    System.Random random = new System.Random();
    HashSet<int> usedRoomIndexes = new HashSet<int>();
    GameObject[] roomList;
    GameObject[] pathList;
    GameObject[] essentialRoomList = new GameObject[5];
    GameObject[] edgeInstances;
    GameObject grid;
    bool[,] occupiedSpace;
    int[,] roomSpace;
    int[,] essentialRoomSpace = new int[5, 2];
    int[,] edges = { {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0} }; // y, x, room_index
    int numberOfRooms;
    int max_h = 0;
    int max_w = 0;
    int matrix_height;
    int matrix_width;
    int max_depth = 2;
    //int max_depth = 5;
    //int max_rooms = 20;
    //int min_rooms = 15;
    int current_rooms = 1;
    int o_x;
    int o_y;

    void RemoveWall(GameObject room, int door) {
        GameObject wallObject;
        GameObject doorObject;
        switch(door) {
            case 0:
                wallObject = room.transform.Find("wallN").gameObject;
                doorObject = room.transform.Find("doorN").gameObject;
                break;
            case 1:
                wallObject = room.transform.Find("wallE").gameObject;
                doorObject = room.transform.Find("doorE").gameObject;
                break;
            case 2:
                wallObject = room.transform.Find("wallS").gameObject;
                doorObject = room.transform.Find("doorS").gameObject;
                break;
            default:
                wallObject = room.transform.Find("wallW").gameObject;
                doorObject = room.transform.Find("doorW").gameObject;
                break;
        }
        wallObject.SetActive(false);
        //doorObject.SetActive(true);
    }

    bool IsOccupied(int room_y, int room_x, int room_index) {
        int room_height = RoomDistance(room_index, 0) / 2;
        int room_width = RoomDistance(room_index, 1) / 2;
        int y_upper_left = room_y - room_height;
        int x_upper_left = room_x - room_width;
        int y_lower_right = room_y + room_height;
        int x_lower_right = room_x + room_width;
        for(int y = y_upper_left; y <= y_lower_right; ++y) {
            for(int x = x_upper_left; x <= x_lower_right; ++x) {
                if (occupiedSpace[y, x]) {
                    return true;
                }
            }
        }
        for(int y = y_upper_left; y <= y_lower_right; ++y) {
            for(int x = x_upper_left; x <= x_lower_right; ++x) {
                occupiedSpace[y, x] = true;
            }
        }
        ShowMap();
        return false;
    }

    int RoomDistance(int room_index, int axis, bool essential=false) {
        if(room_index == -1) {
            return 1;
        }
        if(!essential) {
            return roomSpace[room_index, axis] / 2 + 1;
        }
        return essentialRoomSpace[room_index, axis] / 2 + 1;
    }

    int CheckVEdge(int new_room_y, int new_room_x, int new_room_index, GameObject inst) {
        int edge = -1;
        int dist = RoomDistance(new_room_index, 0);
        if (new_room_y - dist < edges[0, 0]) {
            edge = 0;
            edges[edge, 0] = new_room_y - dist;
            edges[edge, 1] = new_room_x;
            edgeInstances[0] = inst;
        } else {
            if (new_room_y + dist > edges[2, 0]) {
                edge = 2;
                edges[edge, 0] = new_room_y + dist;
                edges[edge, 1] = new_room_x;
                edgeInstances[2] = inst;
            }
        }
        if (edge >= 0) {
            edges[edge, 2] = new_room_index;
        }
        return edge;
    }

    int CheckHEdge(int new_room_y, int new_room_x, int new_room_index, GameObject inst) {
        int edge = -1;
        int dist = RoomDistance(new_room_index, 1);
        if(new_room_x + dist > edges[1, 1]) {
            edge = 1;
            edges[edge, 1] = new_room_x + dist;
            edges[edge, 0] = new_room_y;
            edgeInstances[1] = inst;
        } else {
            if(new_room_x - dist < edges[3, 1]) {
                edge = 3;
                edges[edge, 1] = new_room_x - dist;
                edges[edge, 0] = new_room_y;
                edgeInstances[3] = inst;
            }
        }
        if(edge >= 0) {
            edges[edge, 2] = new_room_index;
        }
        return edge;
    }

    void AddRoom(GameObject currentRoom, int direction, int room_x, int room_y, int room_index, int current_depth) {
        int path_x = room_x;
        int path_y = room_y;
        int new_room_x = room_x;
        int new_room_y = room_y;
        int new_room_index = random.Next(0, numberOfRooms);
        int local_i = 0;
        bool flag = true;
        switch (direction) {
            case 0:
                path_y -= RoomDistance(room_index, 0);
                break;
            case 1:
                path_x -= RoomDistance(room_index, 1);
                break;
            case 2:
                path_y += RoomDistance(room_index, 0);
                break;
            default:
                path_x += RoomDistance(room_index, 1);
                break;
        }
        if (occupiedSpace[path_y, path_x]) {
            return;
        }
        do {
            new_room_index = random.Next(0, numberOfRooms);
            ++local_i;
            if (local_i == 15) {
                flag = false;
                break;
            }
            flag = true;
        } while (usedRoomIndexes.Contains(new_room_index));
        if(!flag) {
            return;
        }
        switch (direction) {
            case 0:
                new_room_y = path_y - RoomDistance(new_room_index, 0);
                break;
            case 1:
                new_room_x = path_x - RoomDistance(new_room_index, 1);
                break;
            case 2:
                new_room_y = path_y + RoomDistance(new_room_index, 0);
                break;
            default:
                new_room_x = path_x + RoomDistance(new_room_index, 1);
                break;
        }
        if (IsOccupied(new_room_y, new_room_x, new_room_index)) {
            return;
        }

        RemoveWall(currentRoom, (direction + 2) % 4);
        GameObject inst_p = AddToGrid(pathList[direction % 2], (path_x - o_x) * 10, (path_y - o_y) * 10);
        occupiedSpace[path_y, path_x] = true;
        GameObject inst_r = AddToGrid(roomList[new_room_index], (new_room_x - o_x) * 10, (new_room_y - o_y) * 10);
        RemoveWall(inst_r, direction);
        ++current_rooms;
        CheckVEdge(new_room_y, new_room_x, new_room_index, inst_r);
        CheckHEdge(new_room_y, new_room_x, new_room_index, inst_r);

        int[] directions = (from num in Enumerable.Range(0, 4) where num != direction select num).ToArray();
        directions = directions.OrderBy(x => random.Next()).ToArray();
        int paths = random.Next(2, 3);
        ++current_depth;
        usedRoomIndexes.Add(new_room_index);
        if(current_depth == max_depth) {
            return;
        }
        for(int i = 0; i < paths; ++i) {
            AddRoom(inst_r, (directions[i] + 2) % 4, new_room_x, new_room_y, new_room_index, current_depth);
            //AddRoom(inst_r, directions[i], new_room_x, new_room_y, new_room_index, current_depth);
        }
    }

    GameObject AddToGrid(GameObject gameObject, int x, int y) {
        GameObject inst = Instantiate(gameObject, new Vector2(x, y), Quaternion.identity);
        grid = GameObject.Find("Grid");
        inst.transform.parent = grid.transform;
        return inst;
    }

    void CreateLevel() {
        essentialRoomList = Resources.LoadAll<GameObject>("EssentialRooms");
        roomList = Resources.LoadAll<GameObject>("Rooms");
        pathList = Resources.LoadAll<GameObject>("Paths");
        numberOfRooms = roomList.Length;
        roomSpace = new int[numberOfRooms, 2];
        for(int i = 0; i < numberOfRooms; ++i) {
            int h = (int)Variables.Object(roomList[i]).Get("h");
            int w = (int)Variables.Object(roomList[i]).Get("w");
            roomSpace[i, 0] = h;
            roomSpace[i, 1] = w;
            max_h = max_h > h ? max_h : h;
            max_w = max_w > w ? max_w : w;
        }
        for(int i = 0; i < 5; ++i) {
            int h = (int)Variables.Object(essentialRoomList[i]).Get("h");
            int w = (int)Variables.Object(essentialRoomList[i]).Get("w");
            essentialRoomSpace[i, 0] = h;
            essentialRoomSpace[i, 1] = w;
        }
        matrix_height = (max_h + 2) * max_depth * 2 + 1;
        matrix_width = (max_w + 2) * max_depth * 2 + 1;
        o_y = matrix_height / 2;
        o_x = matrix_width / 2;
        occupiedSpace = new bool[matrix_height, matrix_width];

        GameObject inst = AddToGrid(essentialRoomList[0], 0, 0);
        occupiedSpace[o_y, o_x] = true;

        int[] directions = { 0, 1, 2, 3 };
        directions = directions.OrderBy(x => random.Next()).ToArray();
        int paths = 4;
        for(int i = 0; i < 4; ++i) {
            edges[i, 0] = o_y;
            edges[i, 1] = o_x;
            edges[i, 2] = -1;
            edgeInstances[i] = inst;
        }
        for(int i = 0; i < paths; ++i) {
            //AddRoom(inst, directions[i], o_x, o_y, -1, 0);
            AddRoom(inst, (directions[i] + 2) % 4, o_x, o_y, -1, 0);
        }
    }

    void FinishLevel() {
        int[] order = { 4, 1, 2, 3 };
        order = order.OrderBy(x => random.Next()).ToArray();
        for(int i = 0; i < 4; ++i) {
            int path_y = (edges[i, 0] - o_y) * 10;
            int path_x = (edges[i, 1] - o_x) * 10;
            int addition;
            if(i % 2 == 1) {
                RemoveWall(edgeInstances[i], i);
                int sign = i < 2 ? 1 : -1;
                for(int j = 0; j < max_w; ++j) {
                    AddToGrid(pathList[1], path_x, path_y);
                    path_x += sign * 10;
                }
                path_x -= sign * 10;
                addition = RoomDistance(order[i], 1, true) * 10;
                GameObject inst = AddToGrid(essentialRoomList[order[i]], path_x + sign * addition, path_y);
                RemoveWall(inst, (i + 2) % 4);
            } else {
                RemoveWall(edgeInstances[i], (i + 2) % 4);
                int sign = i > 0 ? 1 : -1;
                for(int j = 0; j < max_h; ++j) {
                    AddToGrid(pathList[0], path_x, path_y);
                    path_y += sign * 10;
                }
                path_y -= sign * 10;
                addition = RoomDistance(order[i], 0, true) * 10;
                GameObject inst = AddToGrid(essentialRoomList[order[i]], path_x, path_y + sign * addition);
                RemoveWall(inst, i);
            }
        }
    }

    void ShowMap() {
        string path = @"C:\Users\Rotaru\Desktop\unity_map_matrix.txt";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < matrix_height; ++i) {
            for (int j = 0; j < matrix_width; ++j) {
                if(i == o_y && j == o_x) {
                    sb.Append('@');
                } else {
                    sb.Append(occupiedSpace[i, j] ? 'X' : '-');
                }
                //sb.Append(' ');
            }
            sb.Append('\n');
        }
        File.AppendAllText(path, sb.ToString());
    }

    void Start() {
        edgeInstances = new GameObject[4];
        CreateLevel();
        FinishLevel();
        ShowMap();
    }

}
