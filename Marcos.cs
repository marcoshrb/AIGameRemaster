using System;
using System.Drawing;

[Test]
public class Marcos : Player
{
    public Marcos(PointF location) :
        base(location, Color.Aqua, Color.Blue, "Marcos")
    { }

    int i = 0;
    PointF? enemy = null;
    bool isloading = false;

    int searchindex = 0;
    int frame = 0;
    int points = 0;

    int timeShoot = 0;

    protected override void loop()
    {
        frame++;
        if (frame > 300)
        {


            if (EnemiesInInfraRed.Count > 0)
            {
                enemy = EnemiesInInfraRed[0];
            }
            else
            {
                enemy = null;
            }

            if (Energy < 10)
            {
                StopTurbo();
                isloading = true;
                enemy = null;
            }

            if (isloading)
            {
                if (Energy > 20)
                    isloading = false;
                else return;
            }
            if (enemy == null && Energy > 10 && frame % 3 == 0)
                InfraRedSensor(5f * i++);

            else if (enemy != null && Energy > 10)
            {
                if (frame % 5 == 0)
                    InfraRedSensor(enemy.Value);

                float dx = enemy.Value.X - this.Location.X,
                      dy = enemy.Value.Y - this.Location.Y;

                if (dx * dx + dy * dy >= 300f * 300f)
                    StartMove(new PointF(enemy.Value.X + 200, enemy.Value.Y + 200));

                else
                {
                    if(timeShoot > 300){
                        StopMove();
                    }
                    
                    timeShoot++;
                    if (i++ % 2 == 0)
                        Shoot(enemy.Value);
                }
            }
        }


        else {
            
        if (Energy < 10 || frame % 10 == 0)
            return;
        if (EntitiesInStrongSonar == 0)
        {
            StrongSonar();
            points = Points;
        }
        else if (EntitiesInAccurateSonar.Count == 0)
        {
            AccurateSonar();
        }
        else if (FoodsInInfraRed.Count == 0)
        {
            InfraRedSensor(EntitiesInAccurateSonar[searchindex++ % EntitiesInAccurateSonar.Count]);
        }
        else
        {
            StartMove(FoodsInInfraRed[0]);
            if (Points != points)
            {
                StartTurbo();
                StrongSonar();
                StopMove();
                ResetInfraRed();
                ResetSonar();
            }
        }
        }
    }
}
