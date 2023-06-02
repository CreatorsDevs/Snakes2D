using UnityEngine;

public class PlayerA : MonoBehaviour
{
   public Snake snake;

   private void Start() 
   {
      Snake[] snakes = GameObject.FindObjectsOfType<Snake>();

      foreach(Snake snake in snakes)
      {
         if(snake.isSnakeA)
            this.snake = snake;
      }
   }
}
