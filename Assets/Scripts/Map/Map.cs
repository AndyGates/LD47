using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class Map : MonoBehaviour
{
    [SerializeField]
    MapAssets _mapAssets = null;

    [SerializeField]
    GameObject _routePrefab = null;

    public event System.Action<Route> RouteSelected;
    public event System.Action<Node> NodeSelected;
    public event System.Action<Node, bool> NodeHover;

    Dictionary<int, Node> _nodes;
    List<Route> _routes;

    public Vector2 GetNodeCoords(int nodeId)
    {
        return FindNode(nodeId).Coords;
    }

    public int GetNodeType(int nodeId)
    {
        return FindNode(nodeId).Type;
    }

    public bool IsRouteAvailable(int fromId, int toId)
    {
        return FindRoute(fromId, toId) != null;
    }

    public Route FindRoute(int fromId, int toId)
    {
        foreach(Route route in _routes)
        {
            if((route.From == fromId && route.To == toId) || 
            (route.To == fromId && route.From == toId))
            {
                return route;
            }
        }
        return null;
    }

    Node FindNode(int id)
    {
        return _nodes[id];
    }

    public void LoadMap(TextAsset mapData)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(new UnderscoredNamingConvention())
            .Build();

        MapDto mapDto = deserializer.Deserialize<MapDto>(mapData.text);

        // Nodes
        _nodes = new Dictionary<int, Node>(mapDto.Nodes.Count);
        foreach(NodeDto nodeDto in mapDto.Nodes)
        {
            Node node = new Node(
                nodeDto.Name, 
                nodeDto.Id, 
                nodeDto.Type, 
                nodeDto.Coords, 
                nodeDto.Fuel,
                nodeDto.Resources, 
                nodeDto.BuildingSpaces);
            CreateNodeVisual(node);

            _nodes[nodeDto.Id] = node;
        }

        // Routes
        _routes = new List<Route>(mapDto.Routes.Count);
        foreach(RouteDto routeDto in mapDto.Routes)
        {
            Route route = new Route(
                routeDto.From, 
                routeDto.To, 
                routeDto.TravelTime, 
                routeDto.FuelCost,
                routeDto.HealthCost
            );
            CreateRouteVisual(route);

            _routes.Add(route);
        }
    }
    public void MarkNodeRoutesAsDiscovered(int nodeId)
    {
        foreach(Route route in _routes)
        {
            if(route.From == nodeId)
            {
                if(route.State == RouteState.Undiscovered)
                {
                    route.State = RouteState.Untraveled;
                }
            }
        }
    }

    public List<GameAction> GetAvailableActions(int nodeId)
    {
        List<GameAction> actions = new List<GameAction>();
        Node n = FindNode(nodeId);

        if(n.BuildingSpaces > 0) actions.Add(GameAction.Build);
        if(n.Fuel > 0) actions.Add(GameAction.Collect);
        if(n.Resources > 0) actions.Add(GameAction.Mine);

        return actions;
    }

    GameObject CreateNodeVisual(Node node)
    {
        GameObject go = GameObject.Instantiate(_mapAssets.GteNodePrefab(node.Type), node.Coords, Quaternion.identity, transform);

        NodeInteraction interact = go.AddComponent<NodeInteraction>();
        interact.Node = node;
        interact.NodeSelected += OnNodeSelected;
        interact.NodeHover += OnNodeHover;

        return go;
    }

    GameObject CreateRouteVisual(Route route)
    {
        GameObject go = GameObject.Instantiate(_routePrefab, transform);

        RouteInteraction interact = go.AddComponent<RouteInteraction>();
        interact.Route = route;
        interact.RouteSelected += OnRouteSelected;

        RouteVisual visual = go.AddComponent<RouteVisual>();
        visual.Route = route;
        visual.Set(FindNode(route.From).Coords, FindNode(route.To).Coords);

        return go;
    }

    void OnRouteSelected(Route route)
    {
        RouteSelected?.Invoke(route);
    }

    void OnNodeSelected(Node node)
    {
        NodeSelected?.Invoke(node);
    }

    void OnNodeHover(Node node, bool exit)
    {
        NodeHover?.Invoke(node, exit);
    }
}
