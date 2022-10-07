﻿using Module11.UtilityBot.Models;
using System.Collections.Concurrent;

namespace Module11.UtilityBot.Services
{
    public class MemoryStorage : IStorage
    {
        /// <summary>
        /// Хранилище сессий
        /// </summary>
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }
        public Session GetSession(long chatId)
        {
            if (_sessions.ContainsKey(chatId))
            {
                return _sessions[chatId];
            }

            // Создаем и возвращаем новую, если такой не было
            var newSession = new Session() { Choose = "Длина сообщения" };
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}