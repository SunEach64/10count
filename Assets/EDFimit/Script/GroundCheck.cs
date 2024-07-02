using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground"; //�n�ʃ^�O
    private bool isGround = false; //�n�ʂƂ̐ڐG����t���O
    private bool isGroundEnter, isGroundStay, isGroundExit; //���ضް3�킻�ꂼ��̔���t���O

    /// <summary>
    /// �n�ʂƂ̐ڐG����
    /// </summary>
    /// <returns></returns>
    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay) //�ڐG�������A�ڐG�������Ă���ꍇ
        {
            isGround = true; //�ڒn����g�D���[
        }
        else if (isGroundExit) //�ڐG�𗣂ꂽ�ꍇ
        {
            isGround = false; //�ڒn����t�@���X
        }

        //3��t���O�̃��Z�b�g
        isGroundEnter = false; //�ڐG���t���O
        isGroundStay = false; //�ڐG���t���O
        isGroundExit = false; //�ڐG�����t���O

        return isGround; //���肵���ڐG�����Ԃ�
    }

    /// <summary>
    /// �n�ʂƐڐG��
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == groundTag) //GroundCheck�I�u�W�F�N�g��Ground�^�O�̃I�u�W�F�N�g���ڐG����
        {
            isGroundEnter = true; //�ڐG���t���O���g�D���[
        }
    }

    /// <summary>
    /// �n�ʂƐڐG�������Ă���
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == groundTag) //GroundCheck�I�u�W�F�N�g��Ground�^�O�̃I�u�W�F�N�g���ڐG�������Ă���
        {
            isGroundStay = true; //�ڐG���t���O���g�D���[
        }
    }

    /// <summary>
    /// �n�ʂ��痣�ꂽ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == groundTag) //GroundCheck�I�u�W�F�N�g����Ground�^�O�̃I�u�W�F�N�g�����ꂽ
        {
            isGroundExit = true; //�ڐG�����t���O���g�D���[
        }
    }

}
