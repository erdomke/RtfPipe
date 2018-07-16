using System;
using RtfPipe.Support;

namespace RtfPipe.Interpreter
{


  public sealed class RtfTimestampBuilder : RtfElementVisitorBase
  {

    public RtfTimestampBuilder() :
      base( RtfElementVisitorOrder.BreadthFirst )
    {
      Reset();
    }

    public void Reset()
    {
      year = 1970;
      month = 1;
      day = 1;
      hour = 0;
      minutes = 0;
      seconds = 0;
    }

    public DateTime CreateTimestamp()
    {
      if (year == 0 && month == 0 && day == 0 && hour == 0 && minutes == 0)
        return DateTime.MinValue;
      return new DateTime( year, month, day, hour, minutes, seconds );
    }

    protected override void DoVisitTag( IRtfTag tag )
    {
      switch ( tag.Name )
      {
        case RtfSpec.TagInfoYear:
          year = tag.ValueAsNumber;
          break;
        case RtfSpec.TagInfoMonth:
          month = tag.ValueAsNumber;
          break;
        case RtfSpec.TagInfoDay:
          day = tag.ValueAsNumber;
          break;
        case RtfSpec.TagInfoHour:
          hour = tag.ValueAsNumber;
          break;
        case RtfSpec.TagInfoMinute:
          minutes = tag.ValueAsNumber;
          break;
        case RtfSpec.TagInfoSecond:
          seconds = tag.ValueAsNumber;
          break;
      }
    }

    private int year;
    private int month;
    private int day;
    private int hour;
    private int minutes;
    private int seconds;

  }

}

