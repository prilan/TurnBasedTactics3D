using Entitas;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using Random = System.Random;

public class FindCharacterPathService
{
    private IGroup<GameEntity> _characterEntitiesGroup;
    private IGroup<GameEntity> _obstacleEntitiesGroup;
    private Random _random = new Random();

    public FindCharacterPathService(Contexts contexts)
    {
        _characterEntitiesGroup = contexts.game.GetGroup(GameMatcher.Character);
        _obstacleEntitiesGroup = contexts.game.GetGroup(GameMatcher.Obstacle);
    }

    public bool FindPath(Int2 sourcePoint, Int2 destinationPoint, ref HashSet<Int2> handledPoints, out List<Int2> currentPath)
    {
        currentPath = new List<Int2>();

        if (IsFree(destinationPoint)) {
            if (IsNearPoints(sourcePoint, destinationPoint)) {
                currentPath = new List<Int2> {
                    sourcePoint,
                    destinationPoint
                };
                return true;
            } else {
                List<Int2> sortedNearPoints = GetSortedNearApprovedPoints(sourcePoint, destinationPoint, handledPoints);

                if (!sortedNearPoints.Any())
                    return false;

                List<List<Int2>> paths = new List<List<Int2>>();

                foreach (Int2 nearPoint in sortedNearPoints) {
                    handledPoints.Add(nearPoint);
                    bool hasSubPath = FindPath(nearPoint, destinationPoint, ref handledPoints, out currentPath);

                    if (hasSubPath) {
                        if (paths.Any() && currentPath.Count < paths.First().Count) {
                            paths.Clear();
                        }
                        if (!paths.Any() || currentPath.Count == paths.First().Count) {
                            paths.Add(currentPath);
                        }
                    }
                }

                if (paths.Any()) {
                    currentPath = GetRandomFinalPath(sourcePoint, paths);

                    return true;
                } else {
                    return false;
                }
            }
        } else {
            return false;
        }
    }

    private List<Int2> GetSortedNearApprovedPoints(Int2 sourcePoint, Int2 destinationPoint, HashSet<Int2> handledPoints)
    {
        List<Int2> nearPoints = GetNearPoints(sourcePoint);
        List<Int2> nearExistsPoints = nearPoints.Where(point => IsExists(point)).ToList();
        List<Int2> nearExistsFreePoints = nearExistsPoints.Where(point => IsFree(point)).ToList();
        List<Int2> nearExistsFreeNotHandledPoints = nearExistsFreePoints.Where(point => IsNotHandled(point, handledPoints)).ToList();
        List<Int2> sortedNearPoints = GetSortedPoints(nearExistsFreeNotHandledPoints, destinationPoint);

        return sortedNearPoints;
    }

    private bool IsFree(Int2 point)
    {
        foreach (GameEntity characterEntity in _characterEntitiesGroup.GetEntities()) {
            CharacterComponent characterComponent = (CharacterComponent)characterEntity.GetComponent(GameComponentsLookup.Character);
            if (characterComponent.cellPosition.Equals(point))
                return false;
        }
        foreach (GameEntity obstacleEntity in _obstacleEntitiesGroup.GetEntities()) {
            ObstacleComponent obstacleComponent = (ObstacleComponent)obstacleEntity.GetComponent(GameComponentsLookup.Obstacle);
            if (obstacleComponent.cellPosition.Equals(point))
                return false;
        }

        return true;
    }

    private List<Int2> GetNearPoints(Int2 sourcePoint)
    {
        return new List<Int2> {
            sourcePoint + new Int2(-1, 0),
            sourcePoint + new Int2(1, 0),
            sourcePoint + new Int2(0, 1),
            sourcePoint + new Int2(0, -1),
        };
    }

    private bool IsExists(Int2 point)
    {
        return point.x >= 0 && point.x < CommonConsts.FieldWidth && point.y >= 0 && point.y < CommonConsts.FieldHeight;
    }

    private bool IsNotHandled(Int2 point, HashSet<Int2> handledPoints)
    {
        return !handledPoints.Contains(point);
    }

    private bool IsNearPoints(Int2 sourcePoint, Int2 destinationPoint)
    {
        return (Math.Abs(sourcePoint.x - destinationPoint.x) == 1 && sourcePoint.y == destinationPoint.y
             || Math.Abs(sourcePoint.y - destinationPoint.y) == 1 && sourcePoint.x == destinationPoint.x);
    }

    private List<Int2> GetSortedPoints(List<Int2> points, Int2 targetPoint)
    {
        // В приоритете точки, которые ближе к финальной цели
        return points.OrderBy(point => GetDistance(point, targetPoint)).ToList();
    }

    private static float GetDistance(Int2 point1, Int2 point2)
    {
        return (Mathf.Sqrt(Mathf.Pow(point1.x - point2.x, 2)) + Mathf.Pow(point1.y - point2.y, 2));
    }

    private List<Int2> GetRandomFinalPath(Int2 sourcePoint, List<List<Int2>> paths)
    {
        int randomIndex = _random.Next(paths.Count);

        List<Int2> randomPath = paths[randomIndex];
        List<Int2> finalPath = randomPath;

        finalPath.Insert(0, sourcePoint);

        return finalPath;
    }
}

