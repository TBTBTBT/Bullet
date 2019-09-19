using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabModel
{
    private const string RootPath = "Prefab/";
    public enum Path
    {
        [ResourcePath(RootPath + "ui_canvas")]
        UICanvas,
        [ResourcePath(RootPath + "ui_list_vert")]
        VerticalSelectList,
        [ResourcePath(RootPath + "ui_list_item_vert")]
        VerticalSelectItem,
        [ResourcePath(RootPath + "ui_matching")]
        MatchingList,
        [ResourcePath(RootPath + "ui_matching_item")]
        MatchingItem,
        [ResourcePath(RootPath + "ui_button_normal")]
        Button,
        [ResourcePath(RootPath + "ui_label")]
        Label,
        [ResourcePath(RootPath + "map_grid")]
        MapGrid,
        [ResourcePath(RootPath + "map_grid_00")]
        Map00,
        [ResourcePath(RootPath + "map_ui")]
        MapUI,
        [ResourcePath(RootPath + "ui_player")]
        PlayerUI,
        
        [ResourcePath(RootPath + "dice_animation")]
        DiceAnim,

        [ResourcePath(RootPath + "player_view")]
        PlayerView,

        [ResourcePath(RootPath + "drama_view")]
        DramaView,
    }
}