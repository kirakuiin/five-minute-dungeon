using Data;
using Popup;

namespace UI.Gameplay
{
    /// <summary>
    /// 丢弃资源弹窗。
    /// </summary>
    public class ResourceChoiceUIController : CacheablePopupBehaviour
    {
        private ResourceSelectorController _selector;
        
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(ResourceSelectorController selector)
        {
            _selector = selector;
        }
        
        public void OnSelectSword()
        {
            _selector.SetSelectResource(Resource.Sword);
        }

        public void OnSelectShield()
        {
            _selector.SetSelectResource(Resource.Shield);
        }

        public void OnSelectArrow()
        {
            _selector.SetSelectResource(Resource.Arrow);
        }

        public void OnSelectScroll()
        {
            _selector.SetSelectResource(Resource.Scroll);
        }

        public void OnSelectJump()
        {
            _selector.SetSelectResource(Resource.Jump);
        }
    }
}