using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.y - transform.position.y) > float.Epsilon)
        {
            xDir = 0;
            yDir = target.position.y > transform.position.y ? 1 : -1;
            if (Movement(xDir, yDir))
            {
                AttemptMove<Player>(xDir, yDir);
                return;
            }
        }

        if (Mathf.Abs(target.position.x - transform.position.x) > float.Epsilon)
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
            yDir = 0;
            if (Movement(xDir, yDir))
            {
                AttemptMove<Player>(xDir, yDir);
                return;
            }
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        hitPlayer.LoseFood(playerDamage);

        animator.SetTrigger("enemyAttack");

        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }
}