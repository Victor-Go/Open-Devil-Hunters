using System;
using System.Collections.Generic;
using Code.Scripts.Behaviour.UI.Animation;
using UnityEngine;

namespace Code.Scripts.Src.Configurations
{
  public static class UIAnimationConfigurations
  {
    public static readonly Dictionary<string, Func<InitialPositionParameters, Vector2>> InitialPositionFunctions = new()
    {
      {
        "InitialUI/SocialNetworks",
        @params => new Vector2(@params.CurrentDefaultPosition.x + 150, @params.CurrentDefaultPosition.y)
      },
      {
        "InitialUI/Clouds",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y + 150)
      },
      {
        "InitialUI/Moon",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y + 80)
      },
      {
        "InitialUI/Title",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 50)
      },
      {
        "InitialUI/MountainTopRight",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 50)
      },
      {
        "InitialUI/MountainTopLeft",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 100)
      },
      {
        "InitialUI/Devil",
        @params => new Vector2(@params.CurrentDefaultPosition.x - @params.CurrentRectSize.x,
          @params.CurrentDefaultPosition.y - 150)
      },
      {
        "InitialUI/MountainDownLeft",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 150)
      },
      {
        "InitialUI/MainMountain",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 200)
      },
      {
        "InitialUI/MountainCastle",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 150)
      },
      {
        "InitialUI/BottomLayer",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 100)
      },
      {
        "InitialUI/Buttons",
        @params => new Vector2(-@params.CurrentRectSize.x, @params.CurrentDefaultPosition.y)
      },
      {
        "FightPreparationUI2/BackButton",
        @params => new Vector2(@params.CurrentDefaultPosition.x - 100 - @params.CurrentRectSize.x,
          @params.CurrentDefaultPosition.y)
      },
      {
        "FightPreparationUI2/QuestionButton",
        @params => new Vector2(@params.CurrentDefaultPosition.x + 100 - @params.CurrentRectSize.x,
          @params.CurrentDefaultPosition.y)
      },
      {
        "FightPreparationUI2/Player0",
        @params => new Vector2(-@params.CanvasRt.rect.width, @params.CurrentDefaultPosition.y)
      },
      {
        "FightPreparationUI2/Player1",
        @params => new Vector2(@params.CanvasRt.rect.width, @params.CurrentDefaultPosition.y)
      },
      {
        "FightPreparationUI2/Player0Gems",
        @params => new Vector2(@params.CurrentDefaultPosition.x - 150, @params.CurrentDefaultPosition.y)
      },
      {
        "FightPreparationUI2/Player1Gems",
        @params => new Vector2(@params.CurrentDefaultPosition.x + 150, @params.CurrentDefaultPosition.y)
      },
      {
        "FightPreparationUI2/HeroSelection",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CanvasRt.rect.height)
      },
      {
        "FightPreparationUI2/ControlGuide",
        @params => new Vector2(@params.CurrentDefaultPosition.x, -@params.CurrentRectSize.y - 50)
      },
      {
        "FightPreparationUI2/HonorDisplayer",
        @params => new Vector2(@params.CurrentDefaultPosition.x,
          @params.CurrentDefaultPosition.y + @params.CurrentRectSize.y + 10)
      },
      {
        "FightPreparationUI2/Mountain0",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 80)
      },
      {
        "FightPreparationUI2/Mountain1",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 50)
      },
      {
        "FightPreparationUI2/Castle",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 150)
      },
      {
        "FightPreparationUI2/Cloud",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y + 50)
      },
      {
        "FightPreparationUI2/Forest",
        @params => new Vector2(@params.CurrentDefaultPosition.x, @params.CurrentDefaultPosition.y - 60)
      },
      {
        "FightPreparationUI2/BottomButton",
        @params => new Vector2(@params.CurrentDefaultPosition.x,
          -@params.CurrentDefaultPosition.y - @params.CurrentRectSize.y)
      },
    };
  }
}