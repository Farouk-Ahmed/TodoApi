﻿using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ILiveTodoService
    {
        Task<IEnumerable<LiveTodoDTO>> GetAllLiveTodosAsync(int page, int pageSize);
    }
}
