using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;           //The prefab room to instantiate
    public int maxRooms;                    //the max number of rooms to instantiate
    public int minRooms;                    //the min room to instantiate

    private float roomXLenght;              //the x size of the room
    private float roomYLenght;              //the y size of the room
    private bool[,] positions;              //the matrix with the positions where the rooms have to be instantiated
    private List<int[]> instanciatedRooms;  //List with the matrix positions where the rooms had been instantiated
    private int roomsNumber;                //the final number of rooms

    void Start()
    {
        //get sprite x size and y size from the room prefab
        roomXLenght = roomPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        roomYLenght = roomPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        //set the number of rooms randomly between bounds
        roomsNumber = Random.Range(minRooms, maxRooms);
        //instantiate List and matrix
        instanciatedRooms = new List<int[]>();
        positions = new bool[roomsNumber * 2 + 1, roomsNumber * 2 + 1];

        //Initialize matrix of positions
        for (int i = 0; i < positions.GetLength(0); i++)
            for (int j = 0; j < positions.GetLength(1); j++)
                positions[i, j] = false;

        //set the central room in the matrix and the List and instantiate it in game
        positions[roomsNumber, roomsNumber] = true;
        int[] centralRoom = { roomsNumber, roomsNumber };
        instanciatedRooms.Add(centralRoom);
        Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

        //instantiate the rest of the rooms
        roomSet();
    }

    //Once the central room is set in Start, this function will instantiate the rest of the rooms.
    //the function selects randomly a room from the instantiated rooms list and, randomly, selects
    //an adjacent position to it. If the selected position fits boundaries, the the matrix and 
    //List will be updated with the new room and the room will be instantiated in the game.
    private void roomSet()
    {
        while (instanciatedRooms.Count < roomsNumber)
        {
            bool instanciated = false;
            int generalDirection = Random.Range(0, 5);

            while (!instanciated)
            {
                //get random room and valid adjacent position
                //int[] instanciatedRoom = getValidRandomInstanciatedRoom();
                //int[] instanciatedRoom = getValidRecentInstanciatedRoom(3);

                //int[] position = getPosition(instanciatedRoom);
                //int[] position = getLinearPosition(instanciatedRoom);
                //int[] position = getPositionAvoidNeighbours(instanciatedRoom);
                int[] position = getPositionNoNeighbour();
                //int[] position = getDirectionalPosition(instanciatedRoom, generalDirection);

                //if the position is not occupied, then List and matrix are updated and the room is instantiated in game
                if (positions[position[0], position[1]] == false && (position[0] != 0 || position[1] != 0))
                {
                    instanciated = true;
                    positions[position[0], position[1]] = true;
                    instanciatedRooms.Add(position);

                    Vector3 instantiatePosition = new Vector3((position[0] - roomsNumber) * roomXLenght, (position[1] - roomsNumber) * roomYLenght, 0);
                    Instantiate(roomPrefab, instantiatePosition, Quaternion.identity);
                }
            }
        }
    }

    //given the position of an instantiated room, this function returns a valid adjacent position
    private int[] getPosition(int[] instanciatedRoom)
    {
        //initialize variables
        int[] position = { 0, 0 };
        bool isCorner = true;

        //if a corner position is getted, then another oner will be randomly selected
        while (isCorner)
        {
            //set the max range in x and y
            int xMax = 1, yMax = 1;
            int xMin = -1, yMin = -1;

            //check the position will be in matrix boundaries
            if (instanciatedRoom[1] == 0)
                yMax = 0;
            else if (instanciatedRoom[1] == positions.GetLength(1) - 1)
                yMin = 0;
            if (instanciatedRoom[0] == 0)
                xMin = 0;
            else if (instanciatedRoom[0] == positions.GetLength(0) - 1)
                xMax = 0;

            // set the x and y
            int x = Random.Range(xMin, xMax + 1);
            int y = Random.Range(yMin, yMax + 1);

            //set the result
            position[0] = x + instanciatedRoom[0];
            position[1] = y + instanciatedRoom[1];

            //check if is a corner room (a room where the x+=1 and y+=1 from the original, for example)
            isCorner = (x == 0 && y == 0) || (x != 0 && y != 0);
        }
        return position;
    }

    //given the position of an instantiated room, this function returns a valid adjacent position and tries to keep a more linear structure
    private int[] getLinearPosition(int[] instanciatedRoom)
    {
        //initialize variables
        int[] position = { 0, 0 };

        //for the instantiated Room, get if its adjacent instantiated rooms exists, checking if the position can exist in the matrix
        bool topRoomExists = false, leftRoomExists = false, rightRoomExists = false, botRoomExists = false;
        if (instanciatedRoom[1] > 0)
            topRoomExists = positions[instanciatedRoom[0], instanciatedRoom[1] - 1];
        if (instanciatedRoom[0] > 0)
            leftRoomExists = positions[instanciatedRoom[0] - 1, instanciatedRoom[1]];
        if (instanciatedRoom[0] < positions.GetLength(0) - 1)
            rightRoomExists = positions[instanciatedRoom[0] + 1, instanciatedRoom[1]];
        if (instanciatedRoom[1] < positions.GetLength(1) - 1)
            botRoomExists = positions[instanciatedRoom[0], instanciatedRoom[1] + 1];

        //sets some selectors according to the parameters calculated so, the rooms that create a more linear path will be more likely to be chosen
        int topSelect = setSelect(0, botRoomExists, !topRoomExists, 0);
        int leftSelect = setSelect(topSelect, rightRoomExists, !leftRoomExists, 0);
        int rightSelect = setSelect(leftSelect, leftRoomExists, !rightRoomExists, 0);
        int botSelect = setSelect(rightSelect, topRoomExists, !botRoomExists, 0);

        int x = 0, y = 0;
        while (x == 0 && y == 0)
        {
            //using the selectors, a random room is chosen
            float roomSelected = Random.Range(0f, botSelect);
            if (0 < roomSelected && roomSelected < topSelect)
            {
                x = 0; y = -1;
            }
            else if (topSelect < roomSelected && roomSelected < leftSelect)
            {
                x = -1; y = 0;
            }
            else if (leftSelect < roomSelected && roomSelected < rightSelect)
            {
                x = 1; y = 0;
            }
            else if (rightSelect < roomSelected && roomSelected < botSelect)
            {
                x = 0; y = 1;
            }
        }

        //set the result
        position[0] = x + instanciatedRoom[0];
        position[1] = y + instanciatedRoom[1];

        return position;
    }

    //given the position of an instantiated room and a general direction, this function returns a valid adjacent position and tries to follow that direction
    private int[] getDirectionalPosition(int[] instanciatedRoom, int generalDirection)
    {
        //initialize variables
        int[] position = { 0, 0 };

        //for the instantiated Room, get if its adjacent instantiated rooms exists, checking if the position can exist in the matrix
        bool topRoomExists = false, leftRoomExists = false, rightRoomExists = false, botRoomExists = false;

        if (instanciatedRoom[1] > 0)
            topRoomExists = positions[instanciatedRoom[0], instanciatedRoom[1] - 1];
        if (instanciatedRoom[0] > 0)
            leftRoomExists = positions[instanciatedRoom[0] - 1, instanciatedRoom[1]];
        if (instanciatedRoom[0] < positions.GetLength(0) - 1)
            rightRoomExists = positions[instanciatedRoom[0] + 1, instanciatedRoom[1]];
        if (instanciatedRoom[1] < positions.GetLength(1) - 1)
            botRoomExists = positions[instanciatedRoom[0], instanciatedRoom[1] + 1];

        //the selectors are set according to the general direction, the room that follows this general direction will be more likely to be chosen
        int topSelect, leftSelect, rightSelect, botSelect;
        if (generalDirection == 0)
            topSelect = setSelect(0, botRoomExists, !topRoomExists, 10);
        else
            topSelect = setSelect(0, botRoomExists, !topRoomExists, 0);
        if (generalDirection == 1)
            leftSelect = setSelect(topSelect, rightRoomExists, !leftRoomExists, 10);
        else
            leftSelect = setSelect(topSelect, rightRoomExists, !leftRoomExists, 0);
        if (generalDirection == 2)
            rightSelect = setSelect(leftSelect, leftRoomExists, !rightRoomExists, 10);
        else
            rightSelect = setSelect(leftSelect, leftRoomExists, !rightRoomExists, 0);
        if (generalDirection == 3)
            botSelect = setSelect(rightSelect, topRoomExists, !botRoomExists, 10);
        else
            botSelect = setSelect(rightSelect, topRoomExists, !botRoomExists, 0);

        int x = 0, y = 0;
        while (x == 0 && y == 0)
        {
            //using the selectors, a random room is chosen
            float roomSelected = Random.Range(0f, botSelect);
            if (0 < roomSelected && roomSelected < topSelect)
            {
                x = 0; y = -1;
            }
            else if (topSelect < roomSelected && roomSelected < leftSelect)
            {
                x = -1; y = 0;
            }
            else if (leftSelect < roomSelected && roomSelected < rightSelect)
            {
                x = 1; y = 0;
            }
            else if (rightSelect < roomSelected && roomSelected < botSelect)
            {
                x = 0; y = 1;
            }
        }

        //set the result
        position[0] = x + instanciatedRoom[0];
        position[1] = y + instanciatedRoom[1];

        return position;

    }

    //given  the position of an instantiated room, this function returns a valid adjacent room and tries to avoid rooms with 2 or more neighbours
    private int[] getPositionAvoidNeighbours(int[] instanciatedRoom)
    {
        //initialize variables
        int[] position = { 0, 0 };

        //for the instantiated Room, get if its adjacent instantiated rooms exists, checking if the position can exist in the matrix
        bool topRoomExists = false, leftRoomExists = false, rightRoomExists = false, botRoomExists = false;

        if (instanciatedRoom[1] > 0)
            topRoomExists = positions[instanciatedRoom[0], instanciatedRoom[1] - 1];
        if (instanciatedRoom[0] > 0)
            leftRoomExists = positions[instanciatedRoom[0] - 1, instanciatedRoom[1]];
        if (instanciatedRoom[0] < positions.GetLength(0) - 1)
            rightRoomExists = positions[instanciatedRoom[0] + 1, instanciatedRoom[1]];
        if (instanciatedRoom[1] < positions.GetLength(1) - 1)
            botRoomExists = positions[instanciatedRoom[0], instanciatedRoom[1] + 1];

        //for every surronding room to the instanciated room, a bonus is set
        int[] topRoom = { instanciatedRoom[0], instanciatedRoom[1] - 1 };
        int topBonus = setBonus(hasLeftNeighbour(topRoom), hasTopNeighbour(topRoom), hasRightNeighbour(topRoom));
        int[] leftRoom = { instanciatedRoom[0] - 1, instanciatedRoom[1] };
        int leftBonus = setBonus(hasLeftNeighbour(leftRoom), hasTopNeighbour(leftRoom), hasBotNeighbour(leftRoom));
        int[] rightRoom = { instanciatedRoom[0] + 1, instanciatedRoom[1] };
        int rightBonus = setBonus(hasRightNeighbour(rightRoom), hasTopNeighbour(rightRoom), hasBotNeighbour(rightRoom));
        int[] botRoom = { instanciatedRoom[0], instanciatedRoom[1] + 1 };
        int botBonus = setBonus(hasLeftNeighbour(botRoom), hasBotNeighbour(botRoom), hasRightNeighbour(botRoom));

        //then, the selects are also set, according to the calculated bonus
        int topSelect = setSelect(0, botRoomExists, !topRoomExists, topBonus);
        int leftSelect = setSelect(topSelect, rightRoomExists, !leftRoomExists, leftBonus);
        int rightSelect = setSelect(leftSelect, leftRoomExists, !rightRoomExists, rightBonus);
        int botSelect = setSelect(rightSelect, topRoomExists, !botRoomExists, botBonus);

        int x = 0, y = 0;
        while (x == 0 && y == 0)
        {
            //using the selectors, a random room is chosen
            float roomSelected = Random.Range(0f, botSelect);
            if (0 < roomSelected && roomSelected < topSelect)
            {
                x = 0; y = -1;
            }
            else if (topSelect < roomSelected && roomSelected < leftSelect)
            {
                x = -1; y = 0;
            }
            else if (leftSelect < roomSelected && roomSelected < rightSelect)
            {
                x = 1; y = 0;
            }
            else if (rightSelect < roomSelected && roomSelected < botSelect)
            {
                x = 0; y = 1;
            }
        }

        //set the result
        position[0] = x + instanciatedRoom[0];
        position[1] = y + instanciatedRoom[1];

        return position;
    }

    //this function returns a valid position that only has one neighbour and has not been instantiated
    private int[] getPositionNoNeighbour()
    {
        List<int[]> posiblePositions = new List<int[]>();

        //all the instantiated rooms are checked
        foreach (int[] roomPosition in instanciatedRooms)
        {
            //for every instantiated room,
            //the surrounding positions (no corners) are checked.
            //if one sourronding position is not instantiated and has only one neighbour (which will be the already instantiated position)
            //then this sourronding position is added to the list of posible positions
            int x = roomPosition[0];
            int y = roomPosition[1];
            if (positionNotInstantiatedAndHasOneNeighbour(x, y - 1))
                posiblePositions.Add(new int[] { x, y - 1 });
            if (positionNotInstantiatedAndHasOneNeighbour(x - 1, y))
                posiblePositions.Add(new int[] { x - 1, y });
            if (positionNotInstantiatedAndHasOneNeighbour(x + 1, y))
                posiblePositions.Add(new int[] { x + 1, y });
            if (positionNotInstantiatedAndHasOneNeighbour(x, y + 1))
                posiblePositions.Add(new int[] { x, y + 1 });
        }

        //once all instantiated positions has been checked, then a random room from the list is returned
        return posiblePositions[Random.Range(0, posiblePositions.Count)];
    }

    //sets a selection gap to be used by Random.
    //selectStartPoint: sets the point where the gap will begin
    //theOppositeRoomExists: tells if the opposite room is instantiated or not (e.g. a topSelect checks if bot room is instantiated)
    //theRoomCanInstantiate: tells if the room can be instantiated
    //bouns: used to change the probability in case is needed, otherwise, can be set to 0
    private int setSelect(int selectStartPoint, bool theOppositeRoomExists, bool theRoomCanInstantiate, int bonus)
    {
        int resultSelect = selectStartPoint + bonus;
        if (theOppositeRoomExists)
            resultSelect += 10;
        else
            resultSelect += 5;
        if (!theRoomCanInstantiate)
            resultSelect = selectStartPoint;

        return resultSelect;
    }

    //sets a bonus for setSelect according to the neighbour room instances
    private int setBonus(bool neighbour1Exists, bool neighbour2Exists, bool neighbour3Exists)
    {
        int resultBonus = 4;
        if (neighbour1Exists || neighbour2Exists || neighbour3Exists)
            resultBonus = 0;
        return resultBonus;
    }

    //gets the position of a valid instantiated room. the room has to have, at least, one free neighbour
    private int[] getValidRandomInstanciatedRoom()
    {
        bool topRoomCanInstantiate = false, leftRoomCanInstantiate = false, rightRoomCanInstantiate = false, botRoomCanInstantiate = false;
        int[] instanciatedRoom = { 0, 0 };

        //a room is selected randomly and is checked if has free neighbours
        while (!topRoomCanInstantiate && !leftRoomCanInstantiate && !rightRoomCanInstantiate && !botRoomCanInstantiate)
        {
            instanciatedRoom = instanciatedRooms[Random.Range(0, instanciatedRooms.Count)];

            if (instanciatedRoom[1] > 0)
            {
                topRoomCanInstantiate = !positions[instanciatedRoom[0], instanciatedRoom[1] - 1];
            }
            if (instanciatedRoom[0] > 0)
            {
                leftRoomCanInstantiate = !positions[instanciatedRoom[0] - 1, instanciatedRoom[1]];
            }
            if (instanciatedRoom[0] < positions.GetLength(0) - 1)
            {
                rightRoomCanInstantiate = !positions[instanciatedRoom[0] + 1, instanciatedRoom[1]];
            }
            if (instanciatedRoom[1] < positions.GetLength(1) - 1)
            {
                botRoomCanInstantiate = !positions[instanciatedRoom[0], instanciatedRoom[1] + 1];
            }
        }

        return instanciatedRoom;
    }

    //gets the position of a valid instantiated room from the most recent rooms. the room has to have, at least, one free neighbour.
    private int[] getValidRecentInstanciatedRoom(int recent)
    {

        bool topRoomCanInstantiate = false, leftRoomCanInstantiate = false, rightRoomCanInstantiate = false, botRoomCanInstantiate = false;
        int[] instanciatedRoom = { 0, 0 };
        int i = instanciatedRooms.Count;

        //a room is selected randomly and is checked if has free neighbours from the last instantiated ones
        while (!topRoomCanInstantiate && !leftRoomCanInstantiate && !rightRoomCanInstantiate && !botRoomCanInstantiate)
        {
            if (i - recent >= 0)
                instanciatedRoom = instanciatedRooms[Random.Range(i - recent, i)];
            else
                instanciatedRoom = instanciatedRooms[Random.Range(0, i)];

            if (instanciatedRoom[1] > 0)
            {
                topRoomCanInstantiate = !positions[instanciatedRoom[0], instanciatedRoom[1] - 1];
            }
            if (instanciatedRoom[0] > 0)
            {
                leftRoomCanInstantiate = !positions[instanciatedRoom[0] - 1, instanciatedRoom[1]];
            }
            if (instanciatedRoom[0] < positions.GetLength(0) - 1)
            {
                rightRoomCanInstantiate = !positions[instanciatedRoom[0] + 1, instanciatedRoom[1]];
            }
            if (instanciatedRoom[1] < positions.GetLength(1) - 1)
            {
                botRoomCanInstantiate = !positions[instanciatedRoom[0], instanciatedRoom[1] + 1];
            }
            i--;
        }

        return instanciatedRoom;

    }

    //checks if a given room has a top neighbour
    private bool hasTopNeighbour(int[] room)
    {
        return positions[room[0], room[1] - 1];
    }

    //checks if a given room has a left neighbour
    private bool hasLeftNeighbour(int[] room)
    {
        return positions[room[0] - 1, room[1]];
    }

    //checks if a given room has a right neighbour
    private bool hasRightNeighbour(int[] room)
    {
        return positions[room[0] + 1, room[1]];
    }

    //checks if a given room has a bot neighbour
    private bool hasBotNeighbour(int[] room)
    {
        return positions[room[0], room[1] + 1];
    }

    //returns the number of instantiated neighbours a room has
    private int numberOfNeighbours(int[] room)
    {
        int numberOfNeighbours = 0;
        if (hasTopNeighbour(room))
            numberOfNeighbours++;
        if (hasBotNeighbour(room))
            numberOfNeighbours++;
        if (hasLeftNeighbour(room))
            numberOfNeighbours++;
        if (hasRightNeighbour(room))
            numberOfNeighbours++;
        return numberOfNeighbours;
    }

    //checks if a position has been instantiated and if has just one neighbour
    private bool positionNotInstantiatedAndHasOneNeighbour(int x, int y)
    {
        int[] room = { x, y };
        return !positions[room[0], room[1]] && numberOfNeighbours(room) == 1;
    }
}
