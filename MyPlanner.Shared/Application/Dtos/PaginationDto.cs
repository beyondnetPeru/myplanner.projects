﻿namespace MyPlanner.Shared.Application.Dtos
{
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage = 10;
        private readonly int recordsPerPageMax = 50;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                if (value > recordsPerPageMax)
                {
                    recordsPerPage = recordsPerPageMax;
                }
                else
                {
                    recordsPerPage = value;
                }
            }
        }
    }
}
