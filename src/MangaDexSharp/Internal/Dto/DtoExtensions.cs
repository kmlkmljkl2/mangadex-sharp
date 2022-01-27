﻿using MangaDexSharp.Collections;
using MangaDexSharp.Internal.Dto.Resources;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto
{
    internal static class DtoExtensions
    {
        public static ResourceCollection<TResource> MapResponseCollection<TDto, TResource>(
            this ResourcePool pool,
            CollectionResponse<TDto> response)
                where TDto : ResourceDto
                where TResource : MangaDexResource
        {
            List<TResource> resources = new List<TResource>();
            foreach (TDto dto in response.Data)
            {
                if (pool.TryRetrieve(dto, out TResource? resource) && resource != null)
                {
                    resources.Add(resource);
                }
            }

            ResourceCollection<TResource> resultCollection = new ResourceCollection<TResource>(
                resources,
                response.Limit,
                response.Offset,
                response.Total);

            return resultCollection;
        }
    }
}