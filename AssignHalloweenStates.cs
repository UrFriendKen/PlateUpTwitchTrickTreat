using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenTwitchTrickTreat
{
    public class AssignHalloweenStates : GameSystemBase, IModSystem
    {
        TwitchNameList _twitchNameList;
        EntityQuery Customers;
        EntityQuery Groups;

        public override void PostInitialisation()
        {
            base.PostInitialisation();
            _twitchNameList = base.World?.GetExistingSystem<TwitchNameList>();
        }

        protected override void Initialise()
        {
            base.Initialise();
            Customers = GetEntityQuery(new QueryHelper()
                .All(typeof(CCustomer)));
            Groups = GetEntityQuery(new QueryHelper()
                .All(typeof(CCustomerGroup), typeof(CGroupMember)));
        }

        protected override void OnUpdate()
        {
            using NativeArray<Entity> entities = Customers.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity entity = entities[i];
                if (_twitchNameList.IsTrick(entity) && (!Require(entity, out CHalloweenOrder trickOrder) || !trickOrder.IsTrick))
                {
                    Set(entity, CHalloweenOrder.Trick);
                    continue;
                }
                if (_twitchNameList.IsTreat(entity) && (!Require(entity, out CHalloweenOrder treatOrder) || !treatOrder.IsTreat))
                {
                    Set(entity, CHalloweenOrder.Treat);
                    continue;
                }
            }

            using NativeArray<Entity> groupEntities = Groups.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < groupEntities.Length; i++)
            {
                Entity groupEntity = groupEntities[i];
                if (!RequireBuffer(groupEntity, out DynamicBuffer<CGroupMember> buffer))
                    continue;
                foreach (CGroupMember member in buffer)
                {
                    if (Require((Entity)member, out CHalloweenOrder halloweenOrder))
				    {
                        Set(groupEntity, halloweenOrder);
                        break;
                    }
                }
            }
        }
    }
}
