using System;
using ModestTree;

#if !ZEN_NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class ReferenceBinder<TContract> : BinderGeneric<TContract> where TContract : class
    {
        readonly protected SingletonProviderMap _singletonMap;

        public ReferenceBinder(
            DiContainer container, string identifier,
            SingletonProviderMap singletonMap)
            : base(container, identifier)
        {
            _singletonMap = singletonMap;
        }

        public BindingConditionSetter ToTransient()
        {
#if !ZEN_NOT_UNITY3D
            if (_contractType.DerivesFrom(typeof(Component)))
            {
                throw new ZenjectBindException(
                    "Should not use ToTransient for Monobehaviours (when binding type '{0}'), you probably want either ToLookup or ToTransientFromPrefab"
                    .Fmt(_contractType.Name()));
            }
#endif

            return ToProvider(new TransientProvider(_container, typeof(TContract)));
        }

        public BindingConditionSetter ToTransient(Type type)
        {
#if !ZEN_NOT_UNITY3D
            if (type.DerivesFrom(typeof(Component)))
            {
                throw new ZenjectBindException(
                    "Should not use ToTransient for Monobehaviours (when binding type '{0}'), you probably want either ToLookup or ToTransientFromPrefab"
                    .Fmt(type.Name()));
            }
#endif

            return ToProvider(new TransientProvider(_container, type));
        }

        public BindingConditionSetter ToTransient<TConcrete>() where TConcrete : TContract
        {
#if !ZEN_NOT_UNITY3D
            if (typeof(TConcrete).DerivesFrom(typeof(Component)))
            {
                throw new ZenjectBindException(
                    "Should not use ToTransient for Monobehaviours (when binding type '{0}' to '{1}'), you probably want either ToLookup or ToTransientFromPrefab"
                    .Fmt(_contractType.Name(), typeof(TConcrete).Name()));
            }
#endif

            return ToProvider(new TransientProvider(_container, typeof(TConcrete)));
        }

        public BindingConditionSetter ToSingle()
        {
            return ToSingle((string)null);
        }

        public BindingConditionSetter ToSingle(string singletonIdentifier)
        {
            Assert.That(typeof(TContract) != typeof(string));

#if !ZEN_NOT_UNITY3D
            if (_contractType.DerivesFrom(typeof(Component)))
            {
                throw new ZenjectBindException(
                    "Should not use ToSingle for Monobehaviours (when binding type '{0}'), you probably want either ToLookup or ToSinglePrefab or ToSingleGameObject"
                    .Fmt(_contractType.Name()));
            }
#endif

            return ToProvider(_singletonMap.CreateProviderFromType(singletonIdentifier, typeof(TContract)));
        }

        public BindingConditionSetter ToSingle<TConcrete>(string singletonIdentifier)
            where TConcrete : TContract
        {
            Assert.That(typeof(TContract) != typeof(string));

#if !ZEN_NOT_UNITY3D
            if (typeof(TConcrete).DerivesFrom(typeof(Component)))
            {
                throw new ZenjectBindException(
                    "Should not use ToSingle for Monobehaviours (when binding type '{0}' to '{1}'), you probably want either ToLookup or ToSinglePrefab or ToSingleGameObject"
                    .Fmt(_contractType.Name(), typeof(TConcrete).Name()));
            }
#endif

            return ToProvider(_singletonMap.CreateProviderFromType(singletonIdentifier, typeof(TConcrete)));
        }

        public BindingConditionSetter ToSingle(string singletonIdentifier, Type concreteType)
        {
            if (!concreteType.DerivesFromOrEqual(_contractType))
            {
                throw new ZenjectBindException(
                    "Invalid type given during bind command.  Expected type '{0}' to derive from type '{1}'".Fmt(concreteType.Name(), _contractType.Name()));
            }

            return ToProvider(_singletonMap.CreateProviderFromType(singletonIdentifier, concreteType));
        }

        public BindingConditionSetter ToInstance<TConcrete>(TConcrete instance)
            where TConcrete : TContract
        {
            if (ZenUtil.IsNull(instance) && !_container.AllowNullBindings)
            {
                string message;

                if (_contractType == typeof(TConcrete))
                {
                    message = "Received null instance during Bind command with type '{0}'".Fmt(_contractType.Name());
                }
                else
                {
                    message =
                        "Received null instance during Bind command when binding type '{0}' to '{1}'".Fmt(_contractType.Name(), typeof(TConcrete).Name());
                }

                throw new ZenjectBindException(message);
            }

            return ToProvider(new InstanceProvider(typeof(TConcrete), instance));
        }

        public BindingConditionSetter ToSingleInstance<TConcrete>(string singletonIdentifier, TConcrete instance)
            where TConcrete : TContract
        {
            if (ZenUtil.IsNull(instance) && !_container.AllowNullBindings)
            {
                string message;

                if (_contractType == typeof(TConcrete))
                {
                    message = "Received null singleton instance during Bind command with type '{0}'".Fmt(_contractType.Name());
                }
                else
                {
                    message =
                        "Received null singleton instance during Bind command when binding type '{0}' to '{1}'".Fmt(_contractType.Name(), typeof(TConcrete).Name());
                }

                throw new ZenjectBindException(message);
            }

            return ToProvider(_singletonMap.CreateProviderFromInstance(singletonIdentifier, instance));
        }

        public BindingConditionSetter ToSingleMethod<TConcrete>(string singletonIdentifier, Func<InjectContext, TConcrete> method)
            where TConcrete : TContract
        {
            return ToProvider(_singletonMap.CreateProviderFromMethod(singletonIdentifier, method));
        }

        // ToSingleMethod
        public BindingConditionSetter ToSingleMethod<TConcrete>(Func<InjectContext, TConcrete> method)
            where TConcrete : TContract
        {
            return ToSingleMethod<TConcrete>(null, method);
        }

        public BindingConditionSetter ToSingleInstance<TConcrete>(TConcrete instance)
            where TConcrete : TContract
        {
            return ToSingleInstance<TConcrete>(null, instance);
        }

        public BindingConditionSetter ToSingle(Type concreteType)
        {
            return ToSingle(null, concreteType);
        }

        public BindingConditionSetter ToSingle<TConcrete>()
            where TConcrete : TContract
        {
            return ToSingle<TConcrete>(null);
        }

#if !ZEN_NOT_UNITY3D
        // Note that concreteType here could be an interface as well
        public BindingConditionSetter ToSinglePrefab(Type concreteType, string singletonIdentifier, GameObject prefab)
        {
            Assert.That(concreteType.DerivesFromOrEqual<TContract>());

            if (ZenUtil.IsNull(prefab))
            {
                throw new ZenjectBindException(
                    "Received null prefab while binding type '{0}'".Fmt(concreteType.Name()));
            }

            var prefabSingletonMap = _container.Resolve<PrefabSingletonProviderMap>();
            return ToProvider(
                prefabSingletonMap.CreateProvider(singletonIdentifier, concreteType, prefab));
        }

        // Note: Here we assume that the contract is a component on the given prefab
        public BindingConditionSetter ToTransientPrefab<TConcrete>(GameObject prefab)
            where TConcrete : Component, TContract
        {
            // We have to cast to object otherwise we get SecurityExceptions when this function is run outside of unity
            if (ZenUtil.IsNull(prefab))
            {
                throw new ZenjectBindException("Received null prefab while binding type '{0}'".Fmt(typeof(TConcrete).Name()));
            }

            return ToProvider(new GameObjectTransientProviderFromPrefab<TConcrete>(_container, prefab));
        }

        // Creates a new game object and adds the given type as a new component on it
        public BindingConditionSetter ToSingleGameObject(string name)
        {
            if (!_contractType.IsSubclassOf(typeof(Component)))
            {
                throw new ZenjectBindException("Expected UnityEngine.Component derived type when binding type '{0}'".Fmt(_contractType.Name()));
            }

            return ToProvider(new GameObjectSingletonProvider(_contractType, _container, name));
        }

        // Creates a new game object and adds the given type as a new component on it
        public BindingConditionSetter ToSingleGameObject<TConcrete>(string name)
            where TConcrete : Component, TContract
        {
            return ToProvider(new GameObjectSingletonProvider(typeof(TConcrete), _container, name));
        }

        public BindingConditionSetter ToSingleMonoBehaviour<TConcrete>(GameObject gameObject)
            where TConcrete : TContract
        {
            return ToProvider(new MonoBehaviourSingletonProvider(typeof(TConcrete), _container, gameObject));
        }

        // Helper methods
        // ToSinglePrefab
        public BindingConditionSetter ToSinglePrefab(GameObject prefab)
        {
            return ToSinglePrefab(null, prefab);
        }

        public BindingConditionSetter ToSinglePrefab(string identifier, GameObject prefab)
        {
            return ToSinglePrefab<TContract>(identifier, prefab);
        }

        public BindingConditionSetter ToSinglePrefab<TConcrete>(GameObject prefab)
            where TConcrete : TContract
        {
            return ToSinglePrefab<TConcrete>(null, prefab);
        }

        public BindingConditionSetter ToSinglePrefab<TConcrete>(string identifier, GameObject prefab)
            where TConcrete : TContract
        {
            return ToSinglePrefab(typeof(TConcrete), identifier, prefab);
        }

        // ToSingleGameObject
        public BindingConditionSetter ToSingleGameObject()
        {
            return ToSingleGameObject(_contractType.Name());
        }
#endif
    }
}
