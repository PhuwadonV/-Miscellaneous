using UnityEngine;

public abstract class MouseManagerBase : MonoBehaviour {
    public enum ButtonEvent
    {
        Up,
        Down
    }

    [SerializeField]
    protected MouseRayManagerBegin _mouseRayManagerBegin;

    private bool _leftButtonLastStatusIsPressed;
    private Collider _selectedCollider;

    public Collider SelectedCollider
    {
        get { return _selectedCollider; }
        protected set { _selectedCollider = value; }
    }

    private bool _isRaycastHit;
    private Ray _ray;
    private RaycastHit _raycastHit;

    protected abstract bool OnMouseDownOnCollider(ref Ray ray, ref RaycastHit raycastHit);
    protected abstract bool OnMouseUpOnCollider(ref Ray ray, ref RaycastHit raycastHit, Collider selectedCollider);
    protected abstract bool OnSelectedColliderRelease(bool isReleaseOnCollider, ref Ray ray, ref RaycastHit raycastHit, Collider selectedCollider);
    protected abstract void OnMouseDown(ref Ray ray);
    protected abstract void OnMouseUp(ref Ray ray, Collider selectedCollider);
    protected abstract void OnSelectedCollider(Collider selectedCollider);

    protected void MouseRayHitCollider(ButtonEvent mouseStatus)
    {
        if (mouseStatus == ButtonEvent.Down)
        {
            if (OnMouseDownOnCollider(ref _ray, ref _raycastHit)) _selectedCollider = _raycastHit.collider;
            else OnMouseDown(ref _ray);
        }
        else
        {
            if (_selectedCollider != null)
            {
                if (OnSelectedColliderRelease(true, ref _ray, ref _raycastHit, _selectedCollider) && _selectedCollider == _raycastHit.collider) { }
                else if (!OnMouseUpOnCollider(ref _ray, ref _raycastHit, _selectedCollider))
                    OnMouseUp(ref _ray, _selectedCollider);
            }
            else if (!OnMouseUpOnCollider(ref _ray, ref _raycastHit, _selectedCollider)) OnMouseUp(ref _ray, _selectedCollider);
            _selectedCollider = null;
        }
    }

    protected void MouseRayNotHitCollider(ButtonEvent mouseStatus)
    {
        if (mouseStatus == ButtonEvent.Down) OnMouseDown(ref _ray);
        else
        {
            if (_selectedCollider != null) OnSelectedColliderRelease(false, ref _ray, ref _raycastHit, _selectedCollider);
            OnMouseUp(ref _ray, _selectedCollider);
            _selectedCollider = null;
        }
    }

    protected void MouseRayUpdate(ButtonEvent mouseStatus)
    {
        _isRaycastHit = _mouseRayManagerBegin.GetMouseRaycast(out _ray, out _raycastHit);
        if (_isRaycastHit)
            MouseRayHitCollider(mouseStatus);
        else
            MouseRayNotHitCollider(mouseStatus);
    }

    protected void Start()
    {
        _leftButtonLastStatusIsPressed = false;
        _selectedCollider = null;
    }

    protected void Update()
    {
        bool LeftButtonCurrentStatus = Input.GetMouseButton(0);
        if (LeftButtonCurrentStatus != _leftButtonLastStatusIsPressed)
        {
            if(LeftButtonCurrentStatus) MouseRayUpdate(ButtonEvent.Down);
            else MouseRayUpdate(ButtonEvent.Up);
            _leftButtonLastStatusIsPressed = LeftButtonCurrentStatus;
        }
        else if(_selectedCollider != null) OnSelectedCollider(_selectedCollider);
    }

    protected void Reset()
    {
        _mouseRayManagerBegin = GameObject.FindObjectOfType<MouseRayManagerBegin>();
    }
}