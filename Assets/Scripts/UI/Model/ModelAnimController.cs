using UnityEngine;

namespace UI.Model
{
    /// <summary>
    /// 模型动画控制。
    /// </summary>
    public class ModelAnimController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int Lose = Animator.StringToHash("Lose");
        
        private static readonly int Win = Animator.StringToHash("Win");

        public void PlayWin()
        {
            animator.SetTrigger(Win);
        }

        public void PlayLose()
        {
            animator.SetTrigger(Lose);
        }
    }
}