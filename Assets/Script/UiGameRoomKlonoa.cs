using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem;
using UnityEngine;

public class UiGameRoom : MonoBehaviour
{
    [SerializeField] private float _speedMove = 1f;
    [SerializeField] private float _speedRotate = 1f;
    [SerializeField] private CharacterController CharacterController;
    [SerializeField] private Animator _animatorPlayer;
    private Vector2 Direction;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _gravity = 1f;
    [SerializeField] private float _jumpImpulse = 1f;
    [SerializeField] private PlayerCollision _scriptCollision;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Pour Bouger,on crée une fonction "Move" qui prend les info de InputAction pour les mettre dans "Context" Direction est égal à la valeur du Vector2 de "Context" donc le X et le Z du mouvement
    //Vu qu'on ne va pas marcher en haut
    public void Move(InputAction.CallbackContext Context)
    {
        Direction = Context.ReadValue<Vector2>();
    }
    //fin


    //Pour sauter, on créer une fonction "Jump" qui prend les info de InputAction pour les mettre dans "Context"
    //Si le bouton du Jump de l'InputAction est pressé et que le joueur touche un objet qui à pour tag "Ground"
        //Alors changer le booléen "IsJumping", qui lance l'animation de saut, et ajouter la valeur de _jumpImpulse à la vélocité
        //On démarre également une Corroutine (On a pas encore bien vu ce que ça fait)
    public void Jump(InputAction.CallbackContext Context)
    {
        if (Context.phase == InputActionPhase.Started && _scriptCollision.GetColliding() == true)
        {
            _animatorPlayer.SetBool("IsJumping", true);
            _velocity.y = _velocity.y + _jumpImpulse;

            StartCoroutine(StopJumping());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Code pour la gravité, à activer au cas où

        
        if (_scriptCollision.GetColliding() == true && _velocity.y <= 0)
        {
            _velocity.y = 0f;
        }
        else if(_scriptCollision.GetColliding() == false)
        {
            _velocity.y = _velocity.y - _gravity * Time.deltaTime;
        }
        Debug.Log("Velocity Y " + _velocity.y);
        



        Vector3 movement = CharacterController.transform.forward * Direction.y * _speedMove * Time.deltaTime;
        CharacterController.Move(movement);

        CharacterController.transform.Rotate(Vector3.up * Direction.x * _speedRotate * Time.deltaTime);

        if (Direction.magnitude > 0.1 && _animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Jump") == false)
        {
            _animatorPlayer.SetBool("IsRunning", true);
        }
        else
        {
            _animatorPlayer.SetBool("IsRunning", false);
            
        }

    }

    IEnumerator StopJumping()
    {
        yield return new WaitForSeconds(0.1f);
        _animatorPlayer.SetBool("IsJumping", false);
    }
}
