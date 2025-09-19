using Game.Gameplay;
using Game.Gameplay.Components.Unit;
using Game.Gameplay.Components.Grid;
using Game.Gameplay.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class SelectionHighlightSystem : ISystem
{
    private Event<HighlightEvent> _highlightEvent;
    private Filter _units;
    private Stash<PositionComponent> _positionStash;
    private Stash<HighlightComponent> _highlightStash;
    private Stash<SelectedMarker> _selectedMarkerStash;
    private readonly GridContext _gridContext;
    
    public World World { get; set;}
    public SelectionHighlightSystem(GridContext gridContext)
    {
        _gridContext = gridContext;
    }

    public void OnAwake()
    {
        _highlightEvent = World.GetEvent<HighlightEvent>();
        _units = World.Filter.With<PositionComponent>().Build();
        _selectedMarkerStash = World.GetStash<SelectedMarker>();
        _positionStash = World.GetStash<PositionComponent>();
        _highlightStash = World.GetStash<HighlightComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var unitEntity in _units)
        {
            ref var positionComponent = ref _positionStash.Get(unitEntity);
            _gridContext.TryGetTileEntity(positionComponent.position, out var tileEntity);
            if (_selectedMarkerStash.Has(unitEntity))
            {
                if (_highlightStash.Has(tileEntity))
                {
                    ref var highlightComponent = ref _highlightStash.Get(tileEntity);
                    highlightComponent.type = HighlightType.Selected;
                    _highlightEvent.NextFrame(new HighlightEvent()
                    {
                        mapPosition = positionComponent.position
                    });
                    return;
                }
                else
                {
                    _highlightStash.Add(tileEntity) = new HighlightComponent()
                    {
                        position = positionComponent.position,
                        type = HighlightType.Selected
                    };
                    
                    _highlightEvent.NextFrame(new HighlightEvent()
                    {
                        mapPosition = positionComponent.position
                    });
                }
            }
            else
            {
                if (_highlightStash.Has(tileEntity))
                {
                    ref var highlightComponent = ref _highlightStash.Get(tileEntity);
                    if (highlightComponent.type == HighlightType.Selected)
                    {
                        _highlightStash.Remove(tileEntity);
                        
                        _highlightEvent.NextFrame(new HighlightEvent()
                        {
                            mapPosition = positionComponent.position
                        });
                    }
                }
            }
        }
    }

    public void Dispose() { }
}