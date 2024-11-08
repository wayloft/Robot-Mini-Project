using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using ToyViewer;

public class AttachablePartTests
{
    private GameObject testObject;
    private AttachablePart attachablePart;
    private bool detachEventCalled;
    private bool attachEventCalled;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject("TestObject");

        attachablePart = testObject.AddComponent<AttachablePart>();

        Assert.IsNotNull(attachablePart, "AttachablePart component could not be added.");

        attachablePart.OnDetach = new UnityEvent();
        attachablePart.OnAttach = new UnityEvent();
        attachablePart.OnDetach.AddListener(() => detachEventCalled = true);
        attachablePart.OnAttach.AddListener(() => attachEventCalled = true);

        detachEventCalled = false;
        attachEventCalled = false;
    }

    [Test]
    public void Detach_PartDetachesCorrectly()
    {
        Assert.IsNotNull(attachablePart, "AttachablePart component is null at the start of IsDetachable_ReturnsCorrectValue.");

        attachablePart.rootPart = attachablePart;
        attachablePart.Detach();

        Assert.IsTrue(attachablePart.IsDetached(), "Part should be marked as detached.");
        Assert.IsTrue(attachablePart.IsMovable(), "Part should be marked as movable.");
        Assert.IsTrue(detachEventCalled, "Detach event should be called.");
    }

    [Test]
    public void Attach_PartAttachesCorrectly()
    {
        Assert.IsNotNull(attachablePart, "AttachablePart component is null at the start of Attach_PartAttachesCorrectly.");

        GameObject attachPointObject = new GameObject("AttachPoint");
        attachablePart.defaultAttachPoint = attachPointObject.transform;

        attachablePart.Detach();
        attachablePart.Attach();

        Assert.IsFalse(attachablePart.IsDetached(), "Part should not be marked as detached.");
        Assert.IsFalse(attachablePart.IsMovable(), "Part should not be movable after attaching.");
        Assert.IsTrue(attachEventCalled, "Attach event should be called.");
    }

    [Test]
    public void IsDetachable_ReturnsCorrectValue()
    {
        Assert.IsNotNull(attachablePart, "AttachablePart component is null at the start of IsDetachable_ReturnsCorrectValue.");

        Assert.IsFalse(attachablePart.IsDetachable(), "Part should not be detachable by default.");

        typeof(AttachablePart).GetField("isDetachable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(attachablePart, true);
        Assert.IsTrue(attachablePart.IsDetachable(), "Part should be detachable after setting isDetachable to true.");
    }

    [TearDown]
    public void Teardown()
    {
        // Cleanup
        Object.DestroyImmediate(testObject);
    }
}
