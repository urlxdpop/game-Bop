using DG.Tweening;
using System;
using UnityEngine;

public class PlayerBoss : MonoBehaviour, IBoss
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _events;
    [SerializeField] private EventDialogController _eventDialog;

    private bool _isRun;
    private bool _isCaught;
    private bool _isFight;
    private int _numWay;

    private WayLabel[] _wayLabels;

    public event EventHandler OnFight;
    public event EventHandler OnCaught;

    private void Start()
    {
        _wayLabels = _events.GetComponentsInChildren<WayLabel>();
    }

    public void Fight()
    {
        if (!_isFight) {
            _isFight = true;
            OnFight?.Invoke(this, EventArgs.Empty);
        }

        if (_isCaught) { 
            transform.DOKill();
            OnCaught?.Invoke(this, EventArgs.Empty);
            _eventDialog.Interact();
            return;
        }

        MoveToWay();
        _isCaught = IsPlayerInWay();
    }

    public Vector3 GetDir()
    {
        Vector3 direction = (_wayLabels[_numWay].transform.position - transform.position).normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            return direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            return direction.y > 0 ? Vector3.up : Vector3.down;
        }
    }

    private void MoveToWay()
    {
        if (_isRun) return;
        _isRun = true;
        Vector3 targetPos = _wayLabels[_numWay].transform.position;
        transform.DOMove(targetPos, Vector3.Distance(transform.position, targetPos) * _speed).OnComplete(() =>
        {
            _isRun = false;
            _numWay++;
            if (_numWay >= _wayLabels.Length){
                Player.Instance.Die();
            }
        });
    }

    private bool IsPlayerInWay()
    {
        return Player.Instance.CheckCollision(gameObject, Vector3.zero);
    }

    public void SharpAttack()
    {

    }
}
