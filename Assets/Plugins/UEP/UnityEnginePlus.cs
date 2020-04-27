using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Omega+Vector3
//具有角度與力量嚴謹定義的Vector3
//而且角度一定是-180~180
public class Omector3
{
    float _x = 0;
    float _y = 0;
    float _z = 0;
    float _power = 0;
    float _horizontalAngle = 0;
    float _verticalAngle = 0;

    public float Power
    {
        get
        {
            return _power;
        }
        set
        {
            _power = value;
            ProcessOmegaChanged();
        }
    }
    public float HorizontalAngle
    {
        get
        {
            return _horizontalAngle;
        }
        set
        {
            _horizontalAngle = value;
            ProcessOmegaChanged();
        }
    }
    public float VerticalAngle
    {
        get
        {
            return _verticalAngle;
        }
        set
        {
            _verticalAngle = value;
            ProcessOmegaChanged();
        }
    }

    public float X
    {
        get
        {
            return _x;
        }
        set
        {
            _x = value;
            ProcessVectorChanged();
        }
    }
    public float Y
    {
        get
        {
            return _y;
        }
        set
        {
            _y = value;
            ProcessVectorChanged();
        }
    }
    public float Z
    {
        get
        {
            return _z;
        }
        set
        {
            _z = value;
            ProcessVectorChanged();
        }
    }

    public Omector3 DirectionOmector
    {
        get
        {
            return new Omector3(1, _horizontalAngle, _verticalAngle);
        }
    }

    //各種建構子
    public Omector3()
    {
        _power = 0;
        _horizontalAngle = 0;
        _verticalAngle = 0;
        _x = 0;
        _y = 0;
        _z = 0;
    }
    public Omector3(Vector3 vector3)
    {
        _x = vector3.x;
        _y = vector3.y;
        _z = vector3.z;
        ProcessVectorChanged();
    }
    public Omector3(float power, float hori, float vert)
    {
        _power = power;
        _horizontalAngle = hori;
        _verticalAngle = vert;
        ProcessOmegaChanged();
    }

    //當更動了任何x,y,z的情況
    private void ProcessVectorChanged()
    {
        Vector3 newVector = new Vector3(X, Y, Z);
        _power = newVector.magnitude;
        _horizontalAngle = Mathf.Atan2(newVector.z, newVector.x) / Mathf.PI * 180f;
        float horizontalPower = Mathf.Sqrt(newVector.z * newVector.z + newVector.x * newVector.x);
        _verticalAngle = Mathf.Atan2(newVector.y, horizontalPower) / Mathf.PI * 180f;
        _verticalAngle = FunctionDriver.GetRefreshAngles(_verticalAngle);
        _horizontalAngle = FunctionDriver.GetRefreshAngles(_horizontalAngle);
    }

    //當改動了角度和純量時
    private void ProcessOmegaChanged()
    {
        float horizontalPower = _power * Mathf.Cos(_verticalAngle * Mathf.PI / 180f);
        _x = horizontalPower * Mathf.Cos(_horizontalAngle * Mathf.PI / 180f);
        _z = horizontalPower * Mathf.Sin(_horizontalAngle * Mathf.PI / 180f);
        _y = _power * Mathf.Sin(_verticalAngle * Mathf.PI / 180f);
        _verticalAngle = FunctionDriver.GetRefreshAngles(_verticalAngle);
        _horizontalAngle = FunctionDriver.GetRefreshAngles(_horizontalAngle);
    }

    //顯性轉換Omector3->Vector3
    public static implicit operator Vector3(Omector3 omector3)
    {
        return new Vector3(omector3.X, omector3.Y, omector3.Z);
    }

    //顯性轉換Vector3->Omector3
    public static implicit operator Omector3(Vector3 vector3)
    {
        Omector3 omector3 = new Omector3();
        omector3.Power = vector3.magnitude;
        omector3.HorizontalAngle = Mathf.Atan2(vector3.z, vector3.x) / Mathf.PI * 180f;
        float horizontalPower = Mathf.Sqrt(vector3.z * vector3.z + vector3.x * vector3.x);
        omector3.VerticalAngle = Mathf.Atan2(vector3.y, horizontalPower) / Mathf.PI * 180f;
        return omector3;
    }

    //加法運算
    public static Omector3 operator +(Omector3 a, Omector3 b)
    {
        return (Omector3)(((Vector3)a) + ((Vector3)b));
    }

    //減法運算
    public static Omector3 operator -(Omector3 a, Omector3 b)
    {
        return (Omector3)(((Vector3)a) - ((Vector3)b));
    }

    //基底運算
    public static Omector3 operator /(Omector3 a, Omector3 baseOmector)
    {
        //預設是(0,x,-90)
        a.HorizontalAngle += baseOmector.HorizontalAngle;
        a.VerticalAngle += (baseOmector.VerticalAngle+90);
        return a;
    }

    //輸出顯示
    public override string ToString()
    {
        return "Omector3( P = " + Power + " ; " + "H = " + HorizontalAngle + " ; " + "V = " + VerticalAngle + ")";
    }

    public static Omector3 LookAtAngle(Vector3 baseVector, Vector3 targetVector)
    {
        Omector3 ansVector = (Omector3)(targetVector- baseVector);
        return ansVector;
    }

    public static Omector3 LookAtDistance()
    {
        Omector3 returner = new Omector3(0, 0, 0);
        return returner;
    }

}

public class FunctionDriver
{
    //得到重整角度
    public static float GetRefreshAngles(float Angle)
    {
        while (Angle > 180)
            Angle -= 360;
        while (Angle < -180)
            Angle += 360;
        float angle = Angle - 180;
        if (angle > 0)
            Angle = angle - 180;
        else
            Angle = angle + 180;
        return Angle;
    }

    //得到除了Y軸以外的跑步純量
    public static float GetRunningMagnitude(Vector3 vectorIn)
    {
        Vector3 vector = new Vector3(vectorIn.x, 0, vectorIn.z);
        return vector.magnitude;
    }

    public static Omector3 GetRealOmector(Omector3 omectorIn,Omector3 gravityOmector)
    {
        Omector3 returner = new Omector3(omectorIn.Power, omectorIn.HorizontalAngle, omectorIn.VerticalAngle);
        //重力方向，為一個力量為0的Omector
        Omector3 gravityDirection = new Omector3(0, gravityOmector.HorizontalAngle, gravityOmector.VerticalAngle);
        returner /= gravityDirection;
        return returner;
    }
}
