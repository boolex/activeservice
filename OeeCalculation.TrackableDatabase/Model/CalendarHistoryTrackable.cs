﻿using System;
using Production.Abstract.Model;
namespace OeeCalculation.TrackableDatabase.Model
{
    public class CalendarHistoryTrackable : CalendarHistory, ITrackable
    {
        private readonly Track track;
        private readonly INullMask mask;
        public CalendarHistoryTrackable(
            Track track,
            INullMask mask,
            int operatorStationId,
            int calendarHistoryId,
            int changeType,
            int calendar,
            DateTime periodStartTime,
            DateTime changeDate
            ) : base(
              operatorStationId,
            calendarHistoryId,
            changeType,
            calendar,
            periodStartTime,
            changeDate)
        {
            this.track = track;
            this.mask = mask;
        }
        public CalendarHistoryTrackable(INullMask mask, byte[] data, int pos) : this(
            mask: mask,
            track: new Track(data, pos),
            operatorStationId: AxxosBitConverter.ToInt32(data, pos + 9),
            calendarHistoryId: AxxosBitConverter.ToInt32(data, pos + 13),
            changeType: AxxosBitConverter.ToInt32(data, pos + 17),
            calendar: AxxosBitConverter.ToInt32(data, pos + 21),
            periodStartTime: AxxosBitConverter.ToDateTime(data, pos + 25),
            changeDate: AxxosBitConverter.ToDateTime(data, pos + 33)
            )
        { }
        public INullMask Nulls
        {
            get { return mask; }
        }
        public Track Track
        {
            get { return track; }
        }
        public int Size
        {
            get { return 41; }
        }
    }
}
